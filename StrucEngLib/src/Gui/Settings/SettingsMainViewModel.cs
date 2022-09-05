using System;
using System.Collections.Generic;
using StrucEngLib.Model;

namespace StrucEngLib.Gui.Settings
{
    /// <summary>Main View Model for Settings</summary>
    public class SettingsMainViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; }
        public ErrorViewModel ErrorVm { get; set; }
        public Workbench Workbench { get; }

        public SettingsMainViewModel(Workbench wb, MainViewModel mvm)
        {
            Workbench = wb;
            MainViewModel = mvm;
            ErrorVm = new ErrorViewModel();
        }

        public override void UpdateModel()
        {
            new List<ViewModelBase>()
            {
            }.ForEach(vm => vm.UpdateModel());
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

