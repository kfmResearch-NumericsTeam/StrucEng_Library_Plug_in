using System;
using System.Threading.Tasks;

namespace StrucEngLib.Utils
{
    /// <summary>Base class for an async command, will not block executing thread</summary>
    public abstract class AsyncCommandBase : CommandBase
    {
        private bool _isExecuting = false;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            _isExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}