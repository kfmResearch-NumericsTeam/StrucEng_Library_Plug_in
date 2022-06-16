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
        private readonly LinFeMainViewModel _vm;

        public ExecClearModelData(LinFeMainViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object parameter)
        {
            var res = MessageBox.Show("All user data associated with StrucEngLib was deleted.", MessageBoxType.Information);
            
            if (res == DialogResult.Ok)
            {
                MainView.Instance.DisposeUi();
                StrucEngLibPlugin.Instance.ResetData();
                MainView.Instance.LoadUi();    
            }
        }
    }
}