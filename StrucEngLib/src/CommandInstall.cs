using Rhino;
using Rhino.Commands;
using Rhino.UI;
using StrucEngLib.Install;
using Command = Rhino.Commands.Command;

namespace StrucEngLib
{
    public class CommandInstall : Command
    {
        public override string EnglishName => "StrucEngLibInstallDependencies";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Installer i = new Installer();
            i.Owner = RhinoEtoApp.MainWindow;
            i.Show();
            return Result.Success;
        }
    }
}