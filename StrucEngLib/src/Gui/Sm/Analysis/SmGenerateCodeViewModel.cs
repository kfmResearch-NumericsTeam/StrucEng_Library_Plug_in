namespace StrucEngLib.Gui.Sm
{
    /// <summary>Vm for Analysis Control in Sandwich Model</summary>
    public class SmGenerateCodeViewModel : CommonGenerateCodeViewModel
    {
        private readonly SmMainViewModel _vm;
        
        public SmGenerateCodeViewModel(SmMainViewModel vm): base(vm.MainViewModel)
        {
            _vm = vm;
        }
        
        public sealed override void UpdateModel()
        {
            if (_vm.Workbench.SandwichModel != null)
            {
                _vm.Workbench.SandwichModel.FileName = FileName;
            }
            _vm.Workbench.ExecuteInBackground = _executeInBackground;
            _vm.Workbench.ExecuteOnServer = _executeOnServer;
        }

        public sealed override void UpdateViewModel()
        {
            base.UpdateViewModel();
            if (_vm.Workbench.SandwichModel != null)
            {
                FileName = _vm.Workbench.FileName;
            }
            ExecuteInBackground = _vm.Workbench.ExecuteInBackground;
            ExecuteOnServer = _vm.Workbench.ExecuteOnServer;
        }

        protected override void OnInspectModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecShowCode(_vm.MainViewModel, gen.GeneratedCode, _executeInBackground).Execute(null);
            }
        }

        protected override void OnExecuteModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateSmCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecExecuteCode(_vm.MainViewModel, gen.GeneratedCode, _executeInBackground).Execute(null);
            }
        }

        protected override void OnResetData()
        {
            new ExecClearModelData(ExecClearModelData.ClearState.SANDWICH).Execute(null);
        }
    }
}