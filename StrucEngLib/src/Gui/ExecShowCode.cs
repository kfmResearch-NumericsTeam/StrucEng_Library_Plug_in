using System;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary></summary>
    public class ExecShowCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly string _code;

        public ExecShowCode(MainViewModel vm, string code)
        {
            _vm = vm;
            _code = code;
        }

        public override void Execute(object parameter)
        {
            var sourceCode = _code;
            var dialog = new InspectPythonDialog(sourceCode);
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                sourceCode = dialog.Source;

                if (dialog.State == InspectPythonDialog.STATE_EXEC)
                {
                    new ExecExecuteCode(_vm, sourceCode).Execute(null);
                }
            }
        }
    }
}