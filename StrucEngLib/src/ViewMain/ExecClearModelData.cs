using System;
using System.IO;
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
            
        }
    }
}