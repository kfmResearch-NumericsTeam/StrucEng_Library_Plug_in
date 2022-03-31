using Rhino;
using Rhino.Commands;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;
using Rhino.DocObjects;
using System;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;

namespace CodeGenerator
{
    public class CodeGeneratorCommand : Command
    {
        public CodeGeneratorCommand()
        {
            Instance = this;
            Panels.RegisterPanel(PlugIn, typeof(CodeGenPanelView), "Compas Code Generator", null);
        }

        public static CodeGeneratorCommand Instance { get; private set; }

        public override string EnglishName => "CodeGeneratorCommand";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = CodeGenPanelView.PanelId;
            Panels.OpenPanel(panelId);
            string layer = SelectLayer(doc);
            return Result.Success;
        }

        public static string SelectLayer(RhinoDoc doc)
        {
            const ObjectType selectionType = ObjectType.AnyObject;
            GetObject go = new GetObject();
            go.SetCommandPrompt("Select an object");
            go.GeometryFilter = selectionType;
            go.GroupSelect = false;
            go.SubObjectSelect = false;
            go.EnableClearObjectsOnEntry(true);
            go.EnableUnselectObjectsOnExit(false);
            go.DeselectAllBeforePostSelect = true;
            RhinoObject selected = null;

            // select a single element
            for (;;)
            {
                GetResult res = go.GetMultiple(1, 1);
                if (res != GetResult.Object)
                    return null;

                if (go.ObjectsWerePreselected)
                {
                    go.EnablePreSelect(false, true);
                    continue;
                }

                break;
            }

            // keep selection
            RhinoObject rhinoObject = go.Object(0).Object();
            if (null == rhinoObject)
            {
                return null;
            }

            selected = rhinoObject;
            rhinoObject.Select(true);
            doc.Views.Redraw();


            // get name
            int index = selected.Attributes.LayerIndex;
            string name = doc.Layers[index].Name;
            return name;
        }
    }
}