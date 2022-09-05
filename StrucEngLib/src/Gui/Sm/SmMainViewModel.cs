using System;
using System.Collections.Generic;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;

namespace StrucEngLib.Gui.Sm
{
    /// <summary>
    /// Main View Model for Sandwich Model
    /// </summary>
    public class SmMainViewModel : ViewModelBase
    {
        public Workbench Workbench { get; }

        public MainViewModel MainViewModel { get; }
        public ErrorViewModel ErrorVm { get; }
        public SmSettingViewModel SmSettingVm { get; }
        public SmGenerateCodeViewModel GenerateCodeVm { get; }
        public SmAnalysisViewModel AnalysisVm { get; }

        public SmMainViewModel(Workbench wb, MainViewModel mvm)
        {
            Workbench = wb;
            MainViewModel = mvm;
            ErrorVm = new ErrorViewModel();
            SmSettingVm = new SmSettingViewModel(this);
            AnalysisVm = new SmAnalysisViewModel(this);
            GenerateCodeVm = new SmGenerateCodeViewModel(this);
        }

        public override void UpdateModel()
        {
            if (Workbench.SandwichModel == null)
            {
                Workbench.SandwichModel = new SandwichModel();
            }

            new List<ViewModelBase>()
            {
                ErrorVm,
                SmSettingVm,
                AnalysisVm,
                GenerateCodeVm
            }.ForEach(vm => vm.UpdateModel());
        }

        public override void UpdateViewModel()
        {
            new List<ViewModelBase>()
            {
                ErrorVm,
                SmSettingVm,
                AnalysisVm,
                GenerateCodeVm
            }.ForEach(vm => vm.UpdateViewModel());
        }

        public Workbench BuildModel()
        {
            try
            {
                UpdateModel();
            }
            catch (Exception e)
            {
                ErrorVm.ShowException("Error occured while extracting model data from view models", e);
            }

            return Workbench;
        }
    }
}