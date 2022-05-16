using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Step;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Analysis
{
    /// <summary>View Model for Compas Analysis Settings </summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;

        public ObservableCollection<AnalysisItemViewModel> AnalysisViewItems { get; }

        private AnalysisItemViewModel _selectedItem;

        public bool SelectedItemVisible => SelectedItem != null && SelectedItem.Include == true;

        public AnalysisItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedItemVisible));
            }
        }

        // XXX: Keeps track of the step names in the step dialog as we provide an analysis setting for each step
        private readonly HashSet<string> _stepNames = new HashSet<string>();

        private void RegisterEvents(AnalysisItemViewModel vm)
        {
            // XXX: We update SelectedItemVisible if include flag is changed
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_selectedItem.Include))
                {
                    if (_selectedItem.Include == false)
                    {
                        _selectedItem.init();
                    }

                    OnPropertyChanged(nameof(SelectedItemVisible));
                }
            };
        }

        public AnalysisViewModel(MainViewModel vm)
        {
            _vm = vm;
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();

            foreach (var s in vm.Workbench.AnalysisSettings)
            {
                var avm = new AnalysisItemViewModel(s)
                {
                    Include = true
                };
                RegisterEvents(avm);
                AnalysisViewItems.Add(avm);
                _stepNames.Add(s.StepId);
            }

            vm.ListStepVm.StepNames.CollectionChanged += (sender, args) =>
            {
                foreach (string name in args.NewItems ?? Enumerable.Empty<string>().ToList())
                {
                    if (name == ListStepViewModel.StepNameExclude)
                    {
                        continue;
                    }

                    if (!_stepNames.Contains(name))
                    {
                        _stepNames.Add(name);
                        var avm = new AnalysisItemViewModel(new AnalysisSetting())
                        {
                            StepName = name
                        };
                        RegisterEvents(avm);
                        AnalysisViewItems.Add(avm);
                    }
                }
            };
        }

        public override void UpdateModel()
        {
            _vm.Workbench.AnalysisSettings.Clear();
            foreach (var avm in AnalysisViewItems)
            {
                avm.UpdateModel();
                if (avm.Include == true)
                {
                    _vm.Workbench.AnalysisSettings.Add(avm.Model);
                }
            }
        }
    }
}