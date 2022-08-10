using System;
using System.IO;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary></summary>
    public class PythonExecutorMainThread : ICodeExecutor
    {
        public void Execute(string pyCode, Action onDone = null)
        {
            try
            {
                Exec(pyCode);
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
                StrucEngLibLog.Instance.WriteLine(e.StackTrace);
            }
            onDone?.Invoke();
        }

        private void Exec(string code)
        {
            string fileName = Path.GetTempPath() + "strucenglib_" + Guid.NewGuid() + ".py";

            File.WriteAllText(fileName, code);
            StrucEngLibLog.Instance.WriteLine("Executing file: " + fileName + "\n");
            RhinoApp.Wait();
            RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }
    }
}