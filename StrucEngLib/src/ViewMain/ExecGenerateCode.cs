using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Generate Code</summary>
    public class ExecGenerateCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly Workbench _model;

        public bool Success { get; private set; } = false;

        public string GeneratedCode { get; private set; }

        public ExecGenerateCode(MainViewModel vm, Workbench model)
        {
            _vm = vm;
            _model = model;
        }

        public override void Execute(object m)
        {
            Success = false;
            try
            {
                ModelValidator validate = new ModelValidator();
                var valMsgs = validate.ValidateModel(_model);
                if (valMsgs.Count != 0)
                {
                    _vm.ErrorVm.ShowMessages(valMsgs);
                    return;
                }
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during model validation", e);
                return;
            }

            PythonCodeGenerator codeGen = new PythonCodeGenerator(_model);
            var sourceCode = "";
            try
            {
                sourceCode = codeGen.Generate();
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during code generation", e);
                return;
            }

            Success = true;
            GeneratedCode = sourceCode;
        }
    }
}