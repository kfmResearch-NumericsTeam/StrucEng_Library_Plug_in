using System;
using System.Collections.Generic;
using System.Security.Principal;
using Rhino.Geometry.Collections;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model of Sandwich Model
    /// </summary>
    public class SmMainViewModel : ViewModelBase
    {
        public Workbench Workbench { get; }

        public ErrorViewModel ErrorVm { get; }
        public SmSettingViewModel SmSettingVm { get; }
        public SmAnalysisViewModel AnalysisVm { get; }

        public SmMainViewModel(Workbench wb, ErrorViewModel error)
        {
            Workbench = wb;
            ErrorVm = error;
            SmSettingVm = new SmSettingViewModel(this);
            AnalysisVm = new SmAnalysisViewModel(this);
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
                AnalysisVm
            }.ForEach(vm => vm.UpdateModel());
        }

        public override void UpdateViewModel()
        {
            new List<ViewModelBase>()
            {
                ErrorVm,
                SmSettingVm,
                AnalysisVm
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