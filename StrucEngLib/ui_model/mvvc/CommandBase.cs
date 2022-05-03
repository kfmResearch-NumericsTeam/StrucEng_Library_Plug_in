using System;
using System.Windows.Input;

namespace CodeGenerator.mvvc
{
    public abstract class CommandBase: ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;
        
        public abstract void Execute(object parameter);
        
        
        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}