using System;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui
{
    /// <summary>Execute a python command</summary>
    public class ExecExecuteCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly string _code;
        private readonly bool _inBackground;

        public ExecExecuteCode(MainViewModel vm, string code, bool inBackground = false)
        {
            _vm = vm;
            _code = code;
            _inBackground = inBackground;
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
            if (_inBackground)
            {
                new PythonExecutor().ExecuteAsync(source, () =>
                {
                    StrucEngLibLog.Instance.WriteLine("Normalizing Rhino layer text...");
                    RhinoUtils.NormalizeTextHeights(RhinoDoc.ActiveDoc);
                });
            }
            else
            {
                new PythonExecutor().Execute(source, () =>
                {
                    StrucEngLibLog.Instance.WriteLine("Normalizing Rhino layer text...");
                    RhinoUtils.NormalizeTextHeights(RhinoDoc.ActiveDoc);
                });
            }
        }
    }
}