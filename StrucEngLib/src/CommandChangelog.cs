using Rhino;
using Rhino.Commands;
using Rhino.Runtime;
using Rhino.UI;
using Command = Rhino.Commands.Command;

namespace StrucEngLib
{
    /// <summary>
    /// Command to show changelog
    /// </summary>
    public class CommandChangelog : Command
    {
        public static string Url =
            "https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/blob/master/CHANGELOG";

        public override string EnglishName => "StrucEngLibChangelog";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            PythonScript ps = PythonScript.Create();            
            ps.ExecuteScript($"import webbrowser; webbrowser.open('{Url}')");
            return Result.Success;
        }
    }
}