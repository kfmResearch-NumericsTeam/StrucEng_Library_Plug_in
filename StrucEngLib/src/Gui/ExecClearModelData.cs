using System;
using System.IO;
using Eto.Forms;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Clears all user data</summary>
    public class ExecClearModelData : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly ClearState _clearState;

        public enum ClearState
        {
            ALL,
            SANDWICH
        }

        public ExecClearModelData(MainViewModel vm, ClearState clearState = ClearState.ALL)
        {
            _vm = vm;
            _clearState = clearState;
        }

        public override void Execute(object parameter)
        {
            var res = MessageBox.Show("All user data associated with StrucEngLib was deleted.",
                MessageBoxType.Information);

            if (res == DialogResult.Ok)
            {
                if (_clearState == ClearState.ALL)
                {
                    MainView.Instance.DisposeUi();
                    StrucEngLibPlugin.Instance.ResetData();
                    MainView.Instance.LoadUi();
                }
                else
                {
                    MainView.Instance.DisposeUi();
                    StrucEngLibPlugin.Instance.ResetSandwichData();
                    MainView.Instance.LoadUi();
                    
                }
            }
        }
    }
}