using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Generate Code for Sandwich Model</summary>
    public class ExecGenerateSmCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly Workbench _model;

        public bool Success { get; private set; } = false;

        public string GeneratedCode { get; private set; }

        public ExecGenerateSmCode(MainViewModel vm, Workbench model)
        {
            _vm = vm;
            _model = model;
        }

        public override void Execute(object m)
        {
            Success = false;
            try
            {
                var linFeCtx = new LinFeModelValidator().ValidateModel(_model);
                var smCtx = new SmModelValidator().ValidateModel(_model);
                var errorCtxx = new List<ErrorMessageContext>();
                var stop = false;
                var hasMessages = false;

                if (linFeCtx.Messages.Count != 0)
                {
                    hasMessages = true;
                    errorCtxx.Add(linFeCtx);
                    if (linFeCtx.GetByType(MessageType.Error).Count > 0)
                    {
                        // XXX: Only stop if ctx contains error msg, otherwise continue
                        stop = true;
                    }
                }

                if (smCtx.Messages.Count != 0)
                {
                    hasMessages = true;
                    errorCtxx.Add(smCtx);
                    if (smCtx.GetByType(MessageType.Error).Count > 0)
                    {
                        stop = true;
                    }
                }

                if (hasMessages)
                {
                    _vm.ErrorVm.ShowMessages(errorCtxx.ToArray());
                }

                if (stop)
                {
                    return;
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
                sourceCode = codeGen.GenerateSmmCode(_model);
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during code generation of Sandwich Model", e);
                return;
            }

            Success = true;
            GeneratedCode = sourceCode;
        }
    }
}