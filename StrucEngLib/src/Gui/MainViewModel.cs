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
    /// Context class with references to all primary view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ErrorViewModel ErrorVm { get; }
        public LinFeMainViewModel LinFeMainVm { get; }

        private UnhandledExceptionEventHandler _exceptionHandler;

        public Workbench Workbench { get; }

        public MainViewModel() : this(new Workbench())
        {
        }

        public MainViewModel(Workbench wb)
        {
            ErrorVm = new ErrorViewModel();
            LinFeMainVm = new LinFeMainViewModel(wb, ErrorVm);

            Workbench = wb;

            _exceptionHandler = (sender, args) =>
            {
                ErrorVm.ShowException("Something went wrong, we caught an unhandled exception. " +
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
                ErrorVm, LinFeMainVm,
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