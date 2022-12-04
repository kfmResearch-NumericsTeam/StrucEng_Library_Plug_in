using Eto.Forms;

namespace StrucEngLib.Gui
{
    /// <summary>
    /// Common VM to generate code
    /// </summary>
    public abstract class CommonGenerateCodeViewModel : ViewModelBase
    {
        private MainViewModel _vm;
        public RelayCommand CommandInspectModel { get; }
        public RelayCommand CommandExecuteModel { get; }
        public RelayCommand CommandResetData { get; }

        protected bool _executeInBackground = true;

        public bool? ExecuteInBackground
        {
            get => _executeInBackground;
            set
            {
                _executeInBackground = (bool) ((value == null) ? false : value);
                OnPropertyChanged();
            }
        }
        
        protected bool _executeOnServer = false;
        
        public bool? ExecuteOnServer
        {
            get => _executeOnServer;
            set
            {
                _executeOnServer = (bool) ((value == null) ? false : value);
                OnPropertyChanged();
            }
        }

        
        protected string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        protected CommonGenerateCodeViewModel(MainViewModel vm)
        {
            _vm = vm;
            CommandInspectModel = new RelayCommand(OnInspectModel);
            CommandExecuteModel = new RelayCommand(OnExecuteModel);
            CommandResetData = new RelayCommand(OnResetData);
        }


        abstract protected void OnInspectModel();

        abstract protected void OnExecuteModel();

        abstract protected void OnResetData();
    }
}