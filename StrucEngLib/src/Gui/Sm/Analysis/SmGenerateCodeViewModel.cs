using System.Runtime.CompilerServices;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>Vm for Analysis Control in Sandwich Model</summary>
    public class SmGenerateCodeViewModel : ViewModelBase
    {
        private readonly SmMainViewModel _vm;
        public RelayCommand CommandInspectModel { get; }
        public RelayCommand CommandExecuteModel { get; }
        public RelayCommand CommandResetData { get; }
        
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

        public SmGenerateCodeViewModel(SmMainViewModel vm)
        {
            _vm = vm;
            CommandInspectModel = new RelayCommand(OnInspectModel);
            CommandExecuteModel = new RelayCommand(OnExecuteModel);
            CommandResetData = new RelayCommand(OnResetData);
        }
        
        public sealed override void UpdateModel()
        {
            if (_vm.Workbench.SandwichModel != null)
            {
                _vm.Workbench.SandwichModel.FileName = FileName;
            }
        }

        public sealed override void UpdateViewModel()
        {
            if (_vm.Workbench.SandwichModel != null)
            {
                FileName = _vm.Workbench.FileName;
            }   
        }
        
        private void OnInspectModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecShowCode(_vm.MainViewModel, gen.GeneratedCode).Execute(null);
            }
        }

        private void OnExecuteModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecExecuteCode(_vm.MainViewModel, gen.GeneratedCode).Execute(null);
            }
        }

        private void OnResetData()
        {
            new ExecClearModelData(ExecClearModelData.ClearState.SANDWICH).Execute(null);
        }
    }
}