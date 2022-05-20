using Rhino;
using Rhino.Commands;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;

using Command = Rhino.Commands.Command;
namespace StrucEngLib
{
    /// <summary>
    /// Command to show plugin view
    /// </summary>
    public class CommandShowWindow : Command
    {
        public CommandShowWindow()
        {
            Instance = this;
            Panels.RegisterPanel(PlugIn, typeof(MainView), "StrucEngLib", null);
        }

        public static CommandShowWindow Instance { get; private set; }

        public override string EnglishName => "StrucEngLibShowWindow";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = MainView.PanelId;
            Panels.OpenPanel(panelId);
            return Result.Success;
        }
        
        // RhinoApp.WriteLine("Hello world");
        //
        // Workbench model = new Workbench();
        // Set s = new Set();
        // s.Name = "set name";
        // s.SetGeneralDisplacement = new SetGeneralDisplacement();
        // s.SetDisplacementType = SetDisplacementType.GENERAL;
        // model.Layers.Add(s);
        //     
        // var ser = JsonConvert.SerializeObject(model);
        // RhinoApp.WriteLine(ser);
        //
    }
}