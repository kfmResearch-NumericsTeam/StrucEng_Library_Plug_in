using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Sm;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Generate Code for Sandwich Model</summary>
    public class ExecGenerateSmCode : CommandBase
    {
        private readonly SmMainViewModel _mainVm;
        private readonly Workbench _model;

        public bool Success { get; private set; } = false;

        public string GeneratedCode { get; private set; }

        public ExecGenerateSmCode(SmMainViewModel mainVm, Workbench model)
        {
            _mainVm = mainVm;
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
                    _mainVm.ErrorVm.ShowMessages(errorCtxx.ToArray());
                }

                if (stop)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                _mainVm.ErrorVm.ShowException("Error during model validation", e);
                return;
            }

            CompasFea1CodeGenerator codeGen = new CompasFea1CodeGenerator();
            var sourceCode = "";
            try
            {
                sourceCode = codeGen.GenerateSmmCode(_model);
            }
            catch (Exception e)
            {
                _mainVm.ErrorVm.ShowException("Error during code generation of Sandwich Model", e);
                return;
            }

            Success = true;
            GeneratedCode = sourceCode;
        }
    }
}