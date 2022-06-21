using System;
using System.Collections.Generic;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model of LinFe
    /// </summary>
    public class LinFeMainViewModel : ViewModelBase
    {
        public ListLayerViewModel ListLayerVm { get; }
        public LayerDetailsViewModel DetailLayerVm { get; }
        public ListLoadViewModel ListLoadVm { get; }
        public ListStepViewModel ListStepVm { get; }
        public ErrorViewModel ErrorVm { get; }
        public AnalysisViewModel AnalysisVm { get; }
        public Workbench Workbench { get; }

        public LinFeMainViewModel(Workbench wb, ErrorViewModel evm)
        {
            ErrorVm = evm;
            Workbench = wb;
            ListLayerVm = new ListLayerViewModel(this);
            DetailLayerVm = new LayerDetailsViewModel(this);
            ListLoadVm = new ListLoadViewModel(this);
            ListStepVm = new ListStepViewModel(this);
            AnalysisVm = new AnalysisViewModel(this);
        }

        public override void UpdateModel()
        {
            new List<ViewModelBase>()
            {
                ListLayerVm, DetailLayerVm, ListLoadVm, ListStepVm, AnalysisVm
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

        public MainViewModel MainViewModel
        {
            get => StrucEngLibPlugin.Instance.MainViewModel;
        }
    }
}