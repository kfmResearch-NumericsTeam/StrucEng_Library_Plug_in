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

        public ExecClearModelData(MainViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object parameter)
        {
            var res = MessageBox.Show("This will delete all user data from the Rhino Model. Are you sure?", MessageBoxType.Question);
            
            if (res == DialogResult.Ok)
            {
                MainView.Instance.DisposeUi();
                StrucEngLibPlugin.Instance.ResetData();
                MainView.Instance.LoadUi();    
            }
        }
    }
}