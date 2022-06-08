using System;
using System.Collections.Generic;
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

        private UnhandledExceptionEventHandler _exceptionHandler;

        public Workbench Workbench { get; }

        public MainViewModel(): this(new Workbench())
        {
        }
        
        public MainViewModel(Workbench wb)
        {
            ErrorVm = new ErrorViewModel();
            Workbench = wb;
            ListLayerVm = new ListLayerViewModel(this);
            DetailLayerVm = new LayerDetailsViewModel(this);
            ListLoadVm = new ListLoadViewModel(this);
            ListStepVm = new ListStepViewModel(this);
            AnalysisVm = new AnalysisViewModel(this);


            _exceptionHandler = (sender, args) =>
            {
                this.ErrorVm.ShowException("Something went wrong, we caught an unhandled exception. " +
                                           "This is a bug. This will leave the application in an inconsistent state",
                    (Exception) args.ExceptionObject);
            };
            AppDomain.CurrentDomain.UnhandledException += _exceptionHandler;
        }

        public override void Dispose()
        {
            base.Dispose();
            AppDomain.CurrentDomain.UnhandledException -= _exceptionHandler;
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