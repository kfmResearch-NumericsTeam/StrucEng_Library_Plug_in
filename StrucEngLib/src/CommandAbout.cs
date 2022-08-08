using System.Text;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.UI;
using Rhino.UI.Controls;
using StrucEngLib.Utils;
using Command = Rhino.Commands.Command;

namespace StrucEngLib
{
    /// <summary>
    /// Command to show About Screen
    /// </summary>
    public class CommandAbout : Command
    {
        public override string EnglishName => "StrucEngLibAbout";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            StringBuilder b = new StringBuilder();
            b.Append("StrucEngLib Plugin\n");
            b.Append("Version: " + StrucEngLibPlugin.Version);
            MessageBox.Show(b.ToString(), "About");
            return Result.Success;
        }
    }
}