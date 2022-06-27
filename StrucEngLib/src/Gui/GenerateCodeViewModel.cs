using System.Windows.Input;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary></summary>
    public class GenerateCodeViewModel : ViewModelBase
    {
        private readonly GenerateCodeMode _mode;
        private readonly MainViewModel _vm;

        public ICommand CommandClearModel { get; }
        public RelayCommand CommandOnInspectCode { get; }
        public RelayCommand CommandOnExecuteCode { get; }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public enum GenerateCodeMode
        {
            LinFe,
            Sandwich
        }

        public GenerateCodeViewModel(GenerateCodeMode mode, MainViewModel vm)
        {
            _mode = mode;
            _vm = vm;
            CommandOnInspectCode = new RelayCommand(OnInspectCode);
            CommandOnExecuteCode = new RelayCommand(OnExecCode);
            CommandClearModel = new ExecClearModelData(vm);
        }

        private void OnInspectCode()
        {
            var model = _vm.BuildModel();
            switch (_mode)
            {
                case GenerateCodeMode.LinFe:
                {
                    var gen = new ExecGenerateLinFeCode(_vm.LinFeMainVm, model);
                    gen.Execute(null);
                    if (gen.Success)
                    {
                        new ExecShowCode(_vm, gen.GeneratedCode).Execute(null);
                    }

                    break;
                }

                case GenerateCodeMode.Sandwich:
                {
                    var gen = new ExecGenerateSmCode(_vm, model);
                    gen.Execute(null);
                    if (gen.Success)
                    {
                        new ExecShowCode(_vm, gen.GeneratedCode).Execute(null);
                    }

                    break;
                }
                default:
                    break;
            }
        }

        private void OnExecCode()
        {
            var model = _vm.BuildModel();
            switch (_mode)
            {
                case GenerateCodeMode.LinFe:
                {
                    var gen = new ExecGenerateLinFeCode(_vm.LinFeMainVm, model);
                    gen.Execute(null);
                    if (gen.Success)
                    {
                        new ExecExecuteCode(_vm, gen.GeneratedCode).Execute(null);
                    }

                    break;
                }
                case GenerateCodeMode.Sandwich:
                {
                    var gen = new ExecGenerateSmCode(_vm, model);
                    gen.Execute(null);
                    if (gen.Success)
                    {
                        new ExecExecuteCode(_vm, gen.GeneratedCode).Execute(null);
                    }

                    break;
                }
                default:
                    break;
            }
        }
    }
}