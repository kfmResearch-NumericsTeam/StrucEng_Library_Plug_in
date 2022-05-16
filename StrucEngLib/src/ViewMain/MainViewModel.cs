using System;
using System.Collections.Generic;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Analysis;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ListLayerViewModel ListLayerVm { get; }
        public LayerDetailsViewModel DetailLayerVm { get; }
        public ListLoadViewModel ListLoadVm { get; }
        public ListStepViewModel ListStepVm { get; }
        public ErrorViewModel ErrorVm { get; }
        public AnalysisViewModel AnalysisVm { get; }

        public Workbench Workbench { get; }

        public MainViewModel()
        {
            Workbench = new Workbench();
            ListLayerVm = new ListLayerViewModel(this);
            DetailLayerVm = new LayerDetailsViewModel(this);
            ListLoadVm = new ListLoadViewModel(this);
            ListStepVm = new ListStepViewModel(this);
            ErrorVm = new ErrorViewModel();
            AnalysisVm = new AnalysisViewModel(this);
        }

        public override void UpdateModel()
        {
            new List<ViewModelBase>()
            {
                ListLayerVm, DetailLayerVm, ListLoadVm, ListStepVm, ErrorVm, AnalysisVm
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