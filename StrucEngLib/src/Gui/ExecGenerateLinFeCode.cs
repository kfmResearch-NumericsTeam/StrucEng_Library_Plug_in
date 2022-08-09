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
    public class ExecGenerateLinFeCode : CommandBase
    {
        private readonly LinFeMainViewModel _vm;
        private readonly Workbench _model;

        public bool Success { get; private set; } = false;

        public string GeneratedCode { get; private set; }

        public ExecGenerateLinFeCode(LinFeMainViewModel vm, Workbench model)
        {
            _vm = vm;
            _model = model;
        }

        public override void Execute(object m)
        {
            Success = false;
            try
            {
                LinFeModelValidator validate = new LinFeModelValidator();
                var ctx = validate.ValidateModel(_model);
                if (ctx.Messages.Count != 0)
                {
                    var res = _vm.ErrorVm.ShowMessages(ctx);
                    if (res == ErrorViewModel.ViewResult.Cancel)
                    {
                        return;
                    }
                    
                    if (ctx.GetByType(MessageType.Error).Count > 0)
                    {
                        // XXX: Only stop if ctx contains error msg, otherwise continue
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during model validation", e);
                return;
            }

            PythonCodeGenerator codeGen = new PythonCodeGenerator();
            var sourceCode = "";
            try
            {
                sourceCode = codeGen.GenerateLinFeCode(_model);
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