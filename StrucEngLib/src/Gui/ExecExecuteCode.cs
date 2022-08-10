using System;
using System.IO;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Execute a python command</summary>
    public class ExecExecuteCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly string _code;

        public ExecExecuteCode(MainViewModel vm, string code)
        {
            _vm = vm;
            _code = code;
        }

        public override void Execute(object _)
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
            new PythonExecutor().ExecuteAsync(source, () =>
            {
                StrucEngLibLog.Instance.WriteLine("Normalizing Rhino layer text...");
                RhinoUtils.NormalizeTextHeights(RhinoDoc.ActiveDoc);    
            });
        }
    }
}