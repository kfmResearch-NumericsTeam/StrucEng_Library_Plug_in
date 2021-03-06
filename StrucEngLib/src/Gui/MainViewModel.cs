using System;
using System.Collections.Generic;
using StrucEngLib.Model;
using StrucEngLib.Sm;


namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ErrorViewModel ErrorVm { get; }
        public LinFeMainViewModel LinFeMainVm { get; }
        public SmMainViewModel SmMainVm { get; }

        private UnhandledExceptionEventHandler _exceptionHandler;

        public Workbench Workbench { get; }

        public MainViewModel() : this(new Workbench())
        {
        }

        public MainViewModel(Workbench wb)
        {
            Workbench = wb;
            ErrorVm = new ErrorViewModel();
            LinFeMainVm = new LinFeMainViewModel(wb, this);
            SmMainVm = new SmMainViewModel(wb, this);

            _exceptionHandler = (sender, args) =>
            {
                ErrorVm.ShowException("Something went wrong, we caught an unhandled exception. " +
                                      "This is a bug. This will leave the application in an inconsistent state",
                    (Exception) args.ExceptionObject);
            };
            AppDomain.CurrentDomain.UnhandledException += _exceptionHandler;
            LinFeMainVm.UpdateViewModel();
            SmMainVm.UpdateViewModel();
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
                ErrorVm, LinFeMainVm, SmMainVm,
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

        // Stores data into model and updates view model
        public void ReloadLinFe()
        {
            SmMainVm?.UpdateModel();
            LinFeMainVm?.UpdateViewModel();
        }

        // Stores data into model and updates view model
        public void ReloadSandwich()
        {
            LinFeMainVm?.UpdateModel();
            SmMainVm?.UpdateViewModel();
        }
    }
}