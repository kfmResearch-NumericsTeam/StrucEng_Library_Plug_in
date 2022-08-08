using System;
using System.IO;
using System.Threading;
using Rhino;
using Rhino.UI;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Execute python code with Rhino's Python runtime.</summary>
    public class PythonExecutor
    {
        public void ExecuteCode(string snippet)
        {
            var d = new FreezeDialog();
            d.Show();
            
            string fileName = Path.GetTempPath() + "strucenglib_" + Guid.NewGuid() + ".py";
            File.WriteAllText(fileName, snippet);
            try
            {
                StrucEngLibLog.Instance.WriteLine("This operation may take some time.");
                StrucEngLibLog.Instance.WriteLine(
                    "Executing temporary file: " + fileName + "\n");
                
                RhinoApp.Wait();
                Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
                StrucEngLibLog.Instance.WriteLine("Executing code finished.");
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
            }
            finally
            {
                d.Close();
            }
        }
    }
}