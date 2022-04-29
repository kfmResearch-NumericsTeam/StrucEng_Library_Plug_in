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
    /// <summary>
    /// Command to show plugin view
    /// </summary>
    public class CommandShowWindow : Command
    {
        public CommandShowWindow()
        {
            Instance = this;
            Panels.RegisterPanel(PlugIn, typeof(MainViewPanel), "StrucEngLib", null);
        }

        public static CommandShowWindow Instance { get; private set; }

        public override string EnglishName => "StrucEngLibShowWindow";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = MainViewPanel.PanelId;
            Panels.OpenPanel(panelId);
            return Result.Success;
        }
    }
}