using System;
using System.Collections.Generic;
using StrucEngLib.Gui.LinFe.Analysis;
using StrucEngLib.Gui.LinFe.Layer;
using StrucEngLib.Gui.LinFe.Load;
using StrucEngLib.Gui.LinFe.Step;
using StrucEngLib.Model;

namespace StrucEngLib.Gui.LinFe
{
    /// <summary>
    /// Context class with references to all primary view model of LinFe
    /// </summary>
    public class LinFeMainViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; }
        public ListLayerViewModel ListLayerVm { get; }
        public LayerDetailsViewModel DetailLayerVm { get; }
        public ListLoadViewModel ListLoadVm { get; }
        public ListStepViewModel ListStepVm { get; }
        public ErrorViewModel ErrorVm { get; }
        public AnalysisViewModel AnalysisVm { get; }
        public LinFeGenerateCodeViewModel GenerateCodeVm { get; }
        public Workbench Workbench { get; }

        public LinFeMainViewModel(Workbench wb, MainViewModel mvm)
        {
            Workbench = wb;
            MainViewModel = mvm;
            ErrorVm = new ErrorViewModel();
            ListLayerVm = new ListLayerViewModel(this);
            DetailLayerVm = new LayerDetailsViewModel(this);
            ListLoadVm = new ListLoadViewModel(this);
            ListStepVm = new ListStepViewModel(this);
            AnalysisVm = new AnalysisViewModel(this);
            GenerateCodeVm = new LinFeGenerateCodeViewModel(this);
        }

        public override void UpdateModel()
        {
            new List<ViewModelBase>()
            {
                // XXX: LinFe does not have UpdateViewModel implemented (legacy), we update in each view model when needed
                ErrorVm, ListLayerVm, DetailLayerVm, ListLoadVm, ListStepVm, AnalysisVm, GenerateCodeVm
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