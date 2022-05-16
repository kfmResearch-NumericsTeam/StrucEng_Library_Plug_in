using System;
using System.IO;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary></summary>
    public class ExecExecuteCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly string _code;

        public ExecExecuteCode(MainViewModel vm, string code)
        {
            _vm = vm;
            _code = code;
        }

        public override void Execute(object parameter)
        {
            try
            {
                OnGenerateModel(_code);
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during code execution", e);
            }
        }

        protected void OnGenerateModel(string source)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, source);
            Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }
    }
}