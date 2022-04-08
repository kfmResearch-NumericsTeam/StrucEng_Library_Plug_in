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
            Panels.RegisterPanel(PlugIn, typeof(MainViewPanel), "Compas Code Generator", null);
        }

        public static CodeGeneratorCommand Instance { get; private set; }

        public override string EnglishName => "CodeGeneratorCommand";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = MainViewPanel.PanelId;
            Panels.OpenPanel(panelId);
            Rhino.RhinoApp.WriteLine("Plugin loaded");
            return Result.Success;
        }
    }
}