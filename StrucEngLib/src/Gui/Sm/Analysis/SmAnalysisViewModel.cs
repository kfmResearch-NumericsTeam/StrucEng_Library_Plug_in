using System.Runtime.CompilerServices;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>Vm for Analysis Control in Sandwich Model</summary>
    public class SmAnalysisViewModel : ViewModelBase
    {
        private readonly SmMainViewModel _vm;
        public RelayCommand CommandInspectModel { get; }
        public RelayCommand CommandExecuteModel { get; }
        public RelayCommand CommandResetData { get; }

        public SmAnalysisViewModel(SmMainViewModel vm)
        {
            _vm = vm;
            CommandInspectModel = new RelayCommand(OnInspectModel);
            CommandExecuteModel = new RelayCommand(OnExecuteModel);
            CommandResetData = new RelayCommand(OnResetData);
        }

        private void OnInspectModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm.MainViewModel, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecShowCode(_vm.MainViewModel, gen.GeneratedCode).Execute(null);
            }
        }

        private void OnExecuteModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm.MainViewModel, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecExecuteCode(_vm.MainViewModel, gen.GeneratedCode).Execute(null);
            }
        }

        private void OnResetData()
        {
            new ExecClearModelData(_vm.MainViewModel, ExecClearModelData.ClearState.SANDWICH).Execute(null);
        }
    }
}