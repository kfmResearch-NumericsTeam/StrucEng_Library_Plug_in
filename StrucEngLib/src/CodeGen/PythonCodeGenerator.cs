using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>
    /// Code generator to generate python code
    /// </summary>
    public class PythonCodeGenerator
    {
        private string header = $@"
# This is auto generated code by StrucEngLib Plugin {StrucEngLibPlugin.Version}
# Find source at {StrucEngLibPlugin.Website}
# Code generated at {DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}
# Issued by user {Environment.UserName}

from compas_fea.cad import rhino
from compas_fea.structure import ElasticIsotropic
from compas_fea.structure import ElementProperties as Properties
from compas_fea.structure import GeneralStep
from compas_fea.structure import GravityLoad
from compas_fea.structure import AreaLoad
from compas_fea.structure import PointLoad
from compas_fea.structure import GeneralDisplacement
from compas_fea.structure import FixedDisplacement
from compas_fea.structure import FixedDisplacementXX
from compas_fea.structure import FixedDisplacementYY
from compas_fea.structure import FixedDisplacementZZ
from compas_fea.structure import PinnedDisplacement
from compas_fea.structure import RollerDisplacementX
from compas_fea.structure import RollerDisplacementY
from compas_fea.structure import RollerDisplacementZ
from compas_fea.structure import RollerDisplacementXY
from compas_fea.structure import RollerDisplacementYZ
from compas_fea.structure import RollerDisplacementXZ
from compas_fea.structure import ShellSection
from compas_fea.structure import Structure

# Snippets based on code of Andrew Liew (github.com/andrewliew), Benjamin Berger (github.com/Beberger)

name = 'Rahmen'
path = 'C:/Temp/'
mdl = Structure(name=name, path=path)
";

        private const string footer = @"
";

        private readonly Workbench _model;

        public PythonCodeGenerator(Workbench model)
        {
            _model = model;
        }

        private int _loadIdCounter = 0;
        private string RemoveSpaces(string id) => id.Replace(" ", "_");
        private string LoadId() => "load_" + _loadIdCounter++;
        private string LayerId(string id) => RemoveSpaces(id) + "_element";
        private string SetId(string id) => RemoveSpaces(id) + "_set";
        private string SectionId(string id) => RemoveSpaces(id) + "_sec";
        private string PropId(string id) => RemoveSpaces(id) + "_prop";
        private string MatElasticId(string id) => RemoveSpaces(id) + "_mat_elastic";
        private string DispId(string id) => RemoveSpaces(id) + "_disp";

        private string CreateStepName(string id) => "step_" + RemoveSpaces(id);

        private string EmitIfNotEmpty(string key, string value, string comma = ",")
            => string.IsNullOrWhiteSpace(value) ? "" : $" {key}={value}{comma}";

        private string _nl = Environment.NewLine;

        public string Generate()
        {
            _loadIdCounter = 0;
            var b = new StringBuilder();
            b.Append(header);
            EmitLayers(_model.Layers, b);

            var loadNameMap = new Dictionary<Model.Load, string>();
            foreach (var load in _model.Loads ?? Enumerable.Empty<Model.Load>())
            {
                EmitLoad(load, b, loadNameMap);
            }

            EmitSteps(b, loadNameMap);

            b.Append($"{_nl}# == Summary" + _nl);
            b.Append("mdl.summary()" + _nl);

            EmitAnalysisSettings(b, _model);

            b.Append(footer);
            return b.ToString();
        }

        private void EmitLayers(List<Model.Layer> layers, StringBuilder b)
        {
            foreach (var layer in layers ?? Enumerable.Empty<Model.Layer>())
            {
                if (layer.LayerType == LayerType.ELEMENT)
                {
                    EmitElement(layer, b);
                }

                if (layer.LayerType == LayerType.SET)
                {
                    EmitDisplacement(layer, b);
                }
            }
        }

        private void EmitElement(Model.Layer layer, StringBuilder b)
        {
            var element = (Element) layer;
            var layerId = LayerId(element.GetName());
            var layerName = element.GetName();
            b.Append(_nl + $@"# == Element {layerName}" + _nl);
            b.Append(
                $@"rhino.add_nodes_elements_from_layers(mdl, mesh_type='ShellElement', layers=['{layerName}']) {_nl}");

            var mat = element.ElementMaterialElastic;
            var matId = MatElasticId(layerId);
            b.Append($@"mdl.add(ElasticIsotropic(name='{matId}', E={mat.E}, v={mat.V}, p={mat.P}))" + _nl);
            var sectionId = SectionId(layerId);
            b.Append(
                $@"mdl.add(ShellSection(name='{sectionId}', t={element.ElementShellSection.Thickness})) #[mm] " +
                _nl);
            var propId = PropId(layerId);
            b.Append(
                $@"mdl.add(Properties(name='{propId}', material='{matId}', section='{sectionId}', elset='{layerName}'))" +
                _nl);

            if (element.LoadConstraint != null)
            {
                var c = element.LoadConstraint;
                b.Append(
                    $@"mdl.elements[{c.ElementNumber}].axes.update({{'ex': [{c.Ex0}, {c.Ex1}, {c.Ex2}], 'ey': [{c.Ey0}, {c.Ey1}, {c.Ey2}], 'ez': [{c.Ez0}, {c.Ez1}, {c.Ez2}]}}) " +
                    _nl);
            }
        }

        private void EmitDisplacement(Model.Layer layer, StringBuilder b)
        {
            var set = (Set) layer;
            var setName = set.GetName();
            var setId = SetId(setName);
            b.Append(_nl + $@"# == Set {set.GetName()}" + _nl);
            b.Append($@"rhino.add_sets_from_layers(mdl, layers=['{setName}'] ) " + _nl);
            var dispId = DispId(setId);

            var args = new StringBuilder();
            var name = "";
            switch (set.SetDisplacementType)
            {
                case SetDisplacementType.GENERAL:
                    name = "GeneralDisplacement";
                    args.Append(EmitIfNotEmpty("x", set.SetGeneralDisplacement.Ux))
                        .Append(EmitIfNotEmpty("y", set.SetGeneralDisplacement.Uy))
                        .Append(EmitIfNotEmpty("z", set.SetGeneralDisplacement.Uz))
                        .Append(EmitIfNotEmpty("xx", set.SetGeneralDisplacement.Rotx))
                        .Append(EmitIfNotEmpty("yy", set.SetGeneralDisplacement.Roty))
                        .Append(EmitIfNotEmpty("zz", set.SetGeneralDisplacement.Rotz));
                    break;
                case SetDisplacementType.FIXED:
                    name = "FixedDisplacement";
                    break;
                case SetDisplacementType.PINNED:
                    name = "PinnedDisplacement";
                    break;
                case SetDisplacementType.FIXED_XX:
                    name = "FixedDisplacementXX";
                    break;
                case SetDisplacementType.FIXED_YY:
                    name = "FixedDisplacementYY";
                    break;
                case SetDisplacementType.FIXED_ZZ:
                    name = "FixedDisplacementZZ";
                    break;
                case SetDisplacementType.ROLLER_X:
                    name = "RollerDisplacementX";
                    break;
                case SetDisplacementType.ROLLER_Y:
                    name = "RollerDisplacementY";
                    break;
                case SetDisplacementType.ROLLER_Z:
                    name = "RollerDisplacementZ";
                    break;
                case SetDisplacementType.ROLLER_XY:
                    name = "RollerDisplacementXY";
                    break;
                case SetDisplacementType.ROLLER_YZ:
                    name = "RollerDisplacementYZ";
                    break;
                case SetDisplacementType.ROLLER_XZ:
                    name = "RollerDisplacementXZ";
                    break;
                case SetDisplacementType.NONE:
                // XXX: Must have a displacement
                default:
                    throw new Exception("Unknown Displacement type: " + set.SetDisplacementType.ToString());
            }

            b.Append($@"mdl.add([{name}(name='{dispId}', {args} nodes='{setName}')]) " + _nl);
        }

        private void EmitLoad(Model.Load load, StringBuilder b, Dictionary<Model.Load, string> loadNameMap)
        {
            var loadId = "";
            switch (load.LoadType)
            {
                case LoadType.Area:
                {
                    var area = (LoadArea) load;
                    var layersList = StringUtils.ListToPyStr(load.Layers, layer => layer.GetName());
                    b.Append(_nl + $@"# == Load Area {layersList}" + _nl);
                    loadId = LoadId() + "_area";
                    var z = EmitIfNotEmpty("z", area.Z);
                    var x = EmitIfNotEmpty("x", area.X);
                    var y = EmitIfNotEmpty("y", area.Y);
                    b.Append(
                        $@"mdl.add(AreaLoad(name='{loadId}', elements={layersList}, {z} {x} {y} axes='{area.Axes}')) " +
                        _nl);
                    break;
                }
                case LoadType.Gravity:
                {
                    var g = (LoadGravity) load;
                    string layersList = StringUtils.ListToPyStr(load.Layers, layer => layer.GetName());
                    b.Append(_nl + $@"#== Load Gravity {layersList}" + _nl);
                    loadId = LoadId() + "_gravity";
                    b.Append(
                        $@"mdl.add(GravityLoad(name='{loadId}', x={g.X}, y={g.Y}, z={g.Z}, elements={layersList}))" +
                        _nl);
                    break;
                }
                case LoadType.Point:
                {
                    var p = (LoadPoint) load;
                    string layersList = StringUtils.ListToPyStr(load.Layers, layer => layer.GetName());
                    b.Append(_nl + $@"#== Load Point {layersList}" + _nl);
                    loadId = LoadId() + "_point";
                    var z = EmitIfNotEmpty("z", p.Z);
                    var x = EmitIfNotEmpty("x", p.X);
                    var y = EmitIfNotEmpty("y", p.Y);
                    var zz = EmitIfNotEmpty("zz", p.ZZ);
                    var xx = EmitIfNotEmpty("xx", p.XX);
                    var yy = EmitIfNotEmpty("yy", p.YY);
                    b.Append(
                        $@"mdl.add(PointLoad(name='{loadId}', {x} {y} {z} {xx} {yy} {zz} elements={layersList}))" +
                        _nl);
                    break;
                }
                default:
                    RhinoApp.WriteLine("Ignoring unknown load: {0}", load.LoadType);
                    // XXX: Ignore rest
                    break;
            }

            loadNameMap.Add(load, loadId);
        }

        private void EmitSteps(StringBuilder b, Dictionary<Model.Load, string> loadNameMap)
        {
            b.Append(_nl + $@"# == Steps" + _nl);
            /*
             * stepPair: <float order as key, List<Steps> which belong to same order
             * Order is already by key (asc)
             */

            _model.Steps.Sort(delegate(Model.Step s1, Model.Step s2)
            {
                return String.Compare(s1.Order, s2.Order, StringComparison.Ordinal);
            });

            Dictionary<string, Model.Step> stepMap = new Dictionary<string, Model.Step>();
            foreach (var step in _model.Steps)
            {
                var stepName = CreateStepName(step.Order);
                stepMap.Add(stepName, step);
                var loadsNames = new List<string>();
                var dispNames = new List<string>();
                foreach (var stepEntry in step.Entries)
                {
                    switch (stepEntry.Type)
                    {
                        case StepType.Load:
                            loadsNames.Add(loadNameMap[stepEntry.Value as Model.Load]);
                            break;
                        case StepType.Set:
                            dispNames.Add(SetId(((Set) stepEntry.Value).Name));
                            break;
                        default:
                            // XXX: Ignore rest
                            break;
                    }
                }

                var loadStr = "";
                var dispStr = "";
                if (loadsNames.Count > 0)
                {
                    loadStr = $" loads={StringUtils.ListToPyStr(loadsNames, name => name)}, ";
                }

                if (dispNames.Count > 0)
                {
                    dispStr = $" displacements={StringUtils.ListToPyStr(dispNames, name => name)}, ";
                }

                b.Append($@"mdl.add(GeneralStep(name='{stepName}', {loadStr} {dispStr} nlgeom=False))" + _nl);
            }

            if (stepMap.Keys.ToList().Count > 0)
            {
                b.Append($@"mdl.steps_order = {StringUtils.ListToPyStr(stepMap.Keys.ToList(), s => s)} " + _nl);
            }
        }

        private void EmitPlotData(StringBuilder b, string step, string field, bool take = true)
        {
            if (!take)
                return;
            b.Append($@"rhino.plot_data(mdl, step='{step}', field='{field}', cbar_size=1) {_nl}");
        }

        private void EmitAnalysisSettings(StringBuilder b, Workbench model)
        {
            List<AnalysisSetting> sx = new List<AnalysisSetting>();
            foreach (var step in model.Steps)
            {
                if (step.Setting != null && step.Setting.Include)
                {
                    sx.Add(step.Setting);
                }
            }

            b.Append($@"{_nl}# == Run {_nl}");
            var fields = StringUtils.ListToPyStr<string>(new List<string>()
                {
                    sx.Select(s => s.Rf).Contains(true) ? "rf" : "",
                    sx.Select(s => s.Rm).Contains(true) ? "rm" : "",
                    sx.Select(s => s.U).Contains(true) ? "u" : "",
                    sx.Select(s => s.Ur).Contains(true) ? "ur" : "",
                    sx.Select(s => s.Cf).Contains(true) ? "cf" : "",
                    sx.Select(s => s.Cm).Contains(true) ? "cm" : "",
                }.Where(s => s != "").ToList(), (id => id)
            );

            b.Append($@"mdl.analyse_and_extract(software='abaqus', fields={fields}) {_nl}");

            foreach (var s in sx)
            {
                var step = CreateStepName(s.StepId);
                b.Append($@"{_nl}# == Plot Step {step} {_nl}");

                EmitPlotData(b, step, "rfx", s.Rf);
                EmitPlotData(b, step, "rfy", s.Rf);
                EmitPlotData(b, step, "rfz", s.Rf);
                EmitPlotData(b, step, "rfm", s.Rf);

                EmitPlotData(b, step, "rmx", s.Rm);
                EmitPlotData(b, step, "rmy", s.Rm);
                EmitPlotData(b, step, "rmz", s.Rm);
                EmitPlotData(b, step, "rmm", s.Rm);

                EmitPlotData(b, step, "ux", s.U);
                EmitPlotData(b, step, "uy", s.U);
                EmitPlotData(b, step, "uz", s.U);
                EmitPlotData(b, step, "um", s.U);

                EmitPlotData(b, step, "urx", s.Ur);
                EmitPlotData(b, step, "ury", s.Ur);
                EmitPlotData(b, step, "urz", s.Ur);
                EmitPlotData(b, step, "urm", s.Ur);

                EmitPlotData(b, step, "cfx", s.Cf);
                EmitPlotData(b, step, "cfy", s.Cf);
                EmitPlotData(b, step, "cfz", s.Cf);
                EmitPlotData(b, step, "cfm", s.Cf);

                EmitPlotData(b, step, "cmx", s.Cm);
                EmitPlotData(b, step, "cmy", s.Cm);
                EmitPlotData(b, step, "cmz", s.Cm);
                EmitPlotData(b, step, "cmm", s.Cm);
            }
        }
    }
}