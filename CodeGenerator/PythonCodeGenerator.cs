using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.Model;

// ReSharper disable All
// @formatter:off

namespace CodeGenerator
{
    /// <summary>
    /// Code generator to generate python code based on UI input.
    /// </summary>
    public class PythonCodeGenerator
    {
        private string header = $@"
from compas_fea.cad import rhino
from compas_fea.structure import ElasticIsotropic
from compas_fea.structure import ElementProperties as Properties
from compas_fea.structure import GeneralDisplacement
from compas_fea.structure import GeneralStep
from compas_fea.structure import GravityLoad
from compas_fea.structure import AreaLoad
from compas_fea.structure import PointLoad
from compas_fea.structure import PinnedDisplacement
from compas_fea.structure import RollerDisplacementX
from compas_fea.structure import RollerDisplacementY
from compas_fea.structure import RollerDisplacementXY
from compas_fea.structure import ShellSection
from compas_fea.structure import Structure

import sandwichmodel_main as SMM

# Snippets based on code of Andrew Liew (github.com/andrewliew), Benjamin Berger (github.com/Beberger)
# Code Generated by StrucEngLib Plugin {StrucEngLibPlugin.Version}, {StrucEngLibPlugin.Website}

name = 'Rahmen'
path = 'C:/Temp/'
mdl = Structure(name=name, path=path)
";

        private const string footer = @"
# Summary

mdl.summary()

# Run

mdl.analyse_and_extract(software='abaqus', fields=['u','sf','sm'])

# rhino.plot_data(mdl, step='step_load', field='sm1',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sm2',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sm3',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sf4',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sf5',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sf1',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sf2',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='sf3',cbar_size=1)
# rhino.plot_data(mdl, step='step_load', field='um',cbar_size=1)
# #print(mdl.elements[251])
# #print(mdl.elements[100])
# #print(mdl.elements[222])
";

        private readonly Workbench _model;

        public PythonCodeGenerator(Workbench model)
        {
            _model = model;
        }

        private int _loadIdCounter = 0;
        private string LoadId() => "load_" + _loadIdCounter++;
        private string LayerId(string id) => id + "_element";
        private string SetId(string id) => id + "_set";
        private string SectionId(string id) => id + "_sec";
        private string PropId(string id) => id + "_prop";
        private string MatElasticId(string id) => id + "_mat_elastic";
        private string DispId(string id) => id + "_disp";

        private string LayersToStringList(List<Layer> layers)
        {
            StringBuilder b = new StringBuilder();
            b.Append("[ ");
            int n = 0;
            foreach (var l in layers)
            {
                if (n > 0)
                {
                    b.Append(", ");
                }
                b.Append($"'{l.GetName()}'");
                n++;
            }

            b.Append(" ] ");
            return b.ToString();
        }

        private string _nl = Environment.NewLine;

        public string Generate()
        {
            _loadIdCounter = 0;

            StringBuilder b = new StringBuilder();

            b.Append(header);

            foreach (var layer in _model.Layers)
            {
                if (layer.LayerType == LayerType.ELEMENT)
                {
                    var element = (Element) layer;
                    var layerId = LayerId(element.GetName());
                    var layerName = element.GetName();
                    b.Append(_nl + $@"# == Element {layerName}" + _nl);
                    b.Append($@"rhino.add_nodes_elements_from_layers(mdl, mesh_type='ShellElement', layers=['{layerName}'])" + _nl);

                    var mat = element.ElementMaterialElastic;
                    var matId = MatElasticId(layerId);
                    b.Append($@"mdl.add(ElasticIsotropic(name='{matId}', E={mat.E}, v={mat.V}, p={mat.P}))" + _nl);
                    var sectionId = SectionId(layerId);
                    b.Append($@"mdl.add(ShellSection(name='{sectionId}', t={element.ElementShellSection.Thickness})) #[mm] " + _nl);
                    var propId = PropId(layerId);
                    b.Append($@"mdl.add(Properties(name='{propId}', material='{matId}', section='{sectionId}', elset='{layerName}'))" + _nl);
                }

                if (layer.LayerType == LayerType.SET)
                {
                    var set = (Set) layer;
                    var setName = set.GetName();
                    var setId = SetId(setName);
                    b.Append(_nl + $@"# == Set {set.GetName()}" + _nl);
                    b.Append($@"rhino.add_sets_from_layers(mdl, layers=['{setName}'] ) " + _nl);
                    var dispId = DispId(setId);
                    b.Append($@"mdl.add([PinnedDisplacement(name='{dispId}', nodes='{setName}')]) " + _nl);
                }
            }

            foreach (var load in _model.Loads)
            {
                if (load.LoadType == LoadType.Area)
                {
                    var area = (LoadArea) load;
                    string layersList = LayersToStringList(load.Layers);
                    b.Append(_nl + $@"# == Load Area {layersList}" + _nl);
                    string loadId = LoadId() + "_area";
                    b.Append($@"mdl.add(AreaLoad(name='{loadId}', elements={layersList}, z={area.Z}, axes='{area.Axes}')) " + _nl);
                }
                else if (load.LoadType == LoadType.Gravity)
                {
                    string layersList = LayersToStringList(load.Layers);
                    b.Append(_nl + $@"#== Load Gravity {layersList}" + _nl);
                    string loadId = LoadId() + "_gravity";
                    b.Append($@"mdl.add(GravityLoad(name='{loadId}', elements={layersList}))" + _nl);
                }
            }
            
            b.Append(_nl + $@"# == Steps" + _nl);
            
            // TODO:
            // var stepCounter = 0;
            // foreach (var step in _model.Steps)
            // {
            //     var stepName = "step_" + stepCounter++;
            //     if (step.StepType == StepType.Load)
            //     {
            //         var loads = "\'\'";
            //         b.Append($@"mdl.add(GeneralStep(name='{stepName}', loads={loads}, nlgeom=False))" + _nl);
            //     }
            //     else if (step.StepType == StepType.Set)
            //     {
            //         b.Append($@"mdl.add(GeneralStep(name='{stepName}', displacements=['{step.Set.Name}'], nlgeom=False))" + _nl);
            //     }
            // }
            //
            // b.Append($@"mdl.steps_order = ['step_bc','step_load']" + _nl);
            
            b.Append(footer);
            return b.ToString();
        }
        
        private string GroupSteps(Workbench model) {
            var steps = new Dictionary<float, List<Model.Step>>();
            foreach (var step in model.Steps)
            {
                var order = float.Parse(step.Order);
                if (String.IsNullOrEmpty(step.Order)) continue;
                if (steps.ContainsKey(order)) {
                    steps[order].Add(step);
                }
            }
            return "";
        }
    }
}