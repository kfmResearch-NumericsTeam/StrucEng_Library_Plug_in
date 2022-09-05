namespace StrucEngLib.Gui.LinFe.Analysis
{   
    /// <summary>Vm for Analysis Control in Sandwich Model</summary>
    public class LinFeGenerateCodeViewModel : CommonGenerateCodeViewModel
    {
        private LinFeMainViewModel _vm;
        public LinFeGenerateCodeViewModel(LinFeMainViewModel vm): base(vm.MainViewModel)
        {
            _vm = vm;
        }
        
        public override void UpdateModel()
        {
            base.UpdateModel();
            _vm.Workbench.ExecuteInBackground = _executeInBackground;
            _vm.Workbench.FileName = FileName;
        }

        public override void UpdateViewModel()
        {
            base.UpdateViewModel();
            ExecuteInBackground = _vm.Workbench.ExecuteInBackground;
            FileName = _vm.Workbench.FileName;
        }

        protected override void OnInspectModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateLinFeCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecShowCode(_vm.MainViewModel, gen.GeneratedCode, _executeInBackground).Execute(null);
            }
        }

        protected override void OnExecuteModel()
        {
            var model = _vm.BuildModel();
            var gen = new ExecGenerateLinFeCode(_vm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecExecuteCode(_vm.MainViewModel, gen.GeneratedCode, _executeInBackground).Execute(null);
            }
        }

        protected override void OnResetData()
        {
            new ExecClearModelData(ExecClearModelData.ClearState.ALL).Execute(null);
        }
    }
}