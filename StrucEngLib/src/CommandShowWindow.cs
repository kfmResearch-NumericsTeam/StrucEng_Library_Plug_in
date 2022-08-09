using Eto.Drawing;
using Rhino;
using Rhino.Commands;
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
        }

        public static CommandShowWindow Instance { get; private set; }

        public override string EnglishName => "StrucEngLibShowWindow";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Panels.OpenPanel(MainView.PanelId);
            return Result.Success;
        }
    }
}