using System;
using System.IO;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Execute python code with Rhino's Python runtime.</summary>
    public class PythonExecutor
    {
        public void ExecuteCode(string snippet)
        {
            string fileName = Path.GetTempPath() + "strucenglib_" + Guid.NewGuid() + ".py";
            File.WriteAllText(fileName, snippet);
            try
            {
                StrucEngLibLog.Instance.WriteLine("Executing a python command. " +
                                                  "The first run of this may take a while. " +
                                                  "Rhino will freeze until this operation is done");

                StrucEngLibLog.Instance.WriteLine("The temporary file being executed can be found at " + fileName);
                Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
            }
        }
    }
}