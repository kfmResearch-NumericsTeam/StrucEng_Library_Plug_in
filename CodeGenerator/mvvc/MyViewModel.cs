using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CodeGenerator.mvvc
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        public virtual void Dispose() { }
    }
    
    public class MyViewModel: ViewModelBase
    {
        public ICommand TestCommand { get; }

        public MyViewModel()
        {
            TestCommand = new MyViewModelCommands(this);
        }
        
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                    Rhino.RhinoApp.WriteLine("Changed: {0}", value);
                }
            }
        }
    }

    public class MyViewModelCommands : CommandBase
    {

        private readonly MyViewModel _viewModel;
        public MyViewModelCommands(MyViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        
        public override void Execute(object parameter)
        {
            Rhino.RhinoApp.WriteLine("Execute");
        }
    }
}