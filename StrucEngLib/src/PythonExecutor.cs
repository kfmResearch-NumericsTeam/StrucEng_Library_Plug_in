using System;
using System.IO;

namespace StrucEngLib
{
    /// <summary></summary>
    public class PythonExecutor
    {

        public PythonExecutor()
        {
            
        }

        private void WriteLine(string msg)
        {
            try
            {
                Rhino.RhinoApp.WriteLine(msg);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(msg);
            }
        }

        public void ExecuteCode(string snippet)
        {
            string fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, snippet);
            try
            {
                WriteLine("Executing a python command. The first run of this may take a while. Rhino will freeze until this operation is done");
                WriteLine("The temporary file being executed can be found at " + fileName);
                Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }
        
    }
}