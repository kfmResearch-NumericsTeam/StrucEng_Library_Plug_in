using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Invokes compas, draws compas element numbers onto rhino canvas</summary>
    public class ExecElementNumbers : AsyncCommandBase
    {
        private readonly MainViewModel _vm;

        public ExecElementNumbers(MainViewModel vm)
        {
            _vm = vm;
        }

        public override Task ExecuteAsync(object parameter)
        {
            var c = BuildScript();
            RunCode(c);
            return Task.FromResult<object>(null);
        }

        protected string BuildScript()
        {
            var ex = new List<string>();
            foreach (var layer in _vm.ListLayerVm.Layers.ToList())
            {
                if (layer.LayerType == LayerType.ELEMENT)
                {
                    ex.Add(layer.GetName());
                }
            }

            string code = $@"
import rhinoscriptsyntax as rs
import Rhino
import scriptcontext
import System.Guid, System.Drawing.Color

from compas_fea.cad import rhino
from compas_fea.structure import ShellSection
from compas_fea.structure import Structure

#Allgemein
name = 'Rahmen'
path = 'C:/Temp/'

# Structure
mdl = Structure(name=name, path=path)
# Elements
rhino.add_nodes_elements_from_layers(mdl, mesh_type='ShellElement', layers={StringUtils.ListToPyStr(ex, id => id)})

# Add this block
def AddLayer():
    # Add a new layer to the document
    layer_index = scriptcontext.doc.Layers.Add('Element_numbers', System.Drawing.Color.Black)
    return Rhino.Commands.Result.Success
    
if __name__=='__main__':
    AddLayer()
    rs.CurrentLayer('Element_numbers')
    
    # show Element numbers
    for element_num, element in mdl.elements.items():
        # xyz = (mdl.nodes[element.nodes[0]].x, mdl.nodes[element.nodes[0]].y, mdl.nodes[element.nodes[0]].z)
        x = (mdl.nodes[element.nodes[0]].x + mdl.nodes[element.nodes[1]].x + mdl.nodes[element.nodes[2]].x)/3
        y = (mdl.nodes[element.nodes[0]].y + mdl.nodes[element.nodes[1]].y + mdl.nodes[element.nodes[2]].y)/3
        z = (mdl.nodes[element.nodes[0]].z + mdl.nodes[element.nodes[1]].z + mdl.nodes[element.nodes[2]].z)/3
        xyz = (x,y,z)
        rs.AddTextDot(str(element_num), xyz)
";
            return code;
        }

        protected void RunCode(string code)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, code);
            try
            {
                Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
            }
            catch (Exception e)
            {
                // XXX Ignore
            }
        }
    }
}