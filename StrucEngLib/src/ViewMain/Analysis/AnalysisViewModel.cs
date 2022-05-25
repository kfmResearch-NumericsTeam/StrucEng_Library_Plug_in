using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eto.Forms;
using StrucEngLib.Model;
using StrucEngLib.Step;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Analysis
{
    /// <summary>VM to attach settings to a step. For each aggregated step we create a settings object</summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;

        public ObservableCollection<AnalysisItemViewModel> AnalysisViewItems { get; }

        private AnalysisItemViewModel _selectedItem;

        public bool SelectedItemVisible => SelectedItem != null && SelectedItem.Include == true;

        private AnalysisItemViewModel ViewModelBySettingsObj(AnalysisSetting setting)
        {
            if (setting == null) return null;
            foreach (var settingVm in AnalysisViewItems)
            {
                if (settingVm.Model == setting)
                {
                    return settingVm;
                }
            }

            return null;
        }

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

        public override void UpdateViewModel()
        {
            AnalysisViewItems.Clear();
            foreach (var s in _vm.Workbench.Steps)
            {
                if (s.Setting == null)
                {
                    s.Setting = new AnalysisSetting();
                }

                var avm = new AnalysisItemViewModel(s.Setting)
                {
                    Include = true,
                    StepName = s.Order
                };
                RegisterEvents(avm);
                AnalysisViewItems.Add(avm);
            }
        }

        private void OnNewStep(IList steps)
        {
            if (steps == null) return;
            foreach (AggregatedStepViewModel s in steps)
            {
                if (s.Model.Setting == null)
                {
                    s.Model.Setting = new AnalysisSetting();
                }

                var avm = new AnalysisItemViewModel(s.Model.Setting)
                {
                    StepName = s.Order
                };
                AnalysisViewItems.Add(avm);
                RegisterEvents(avm);
            }
        }

        private void OnRemoveStep(IList steps)
        {
            if (steps == null) return;
            foreach (AggregatedStepViewModel s in steps)
            {
                var vm = ViewModelBySettingsObj(s.Model.Setting);
                AnalysisViewItems.Remove(vm);
            }
        }

        public AnalysisViewModel(MainViewModel vm)
        {
            _vm = vm;
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();
            vm.ListStepVm.AggregatedSteps.CollectionChanged += (sender, args) =>
            {
                OnNewStep(args.NewItems);
                OnRemoveStep(args.OldItems);
            };
            UpdateViewModel();
        }

        public override void UpdateModel()
        {
            foreach (var avm in AnalysisViewItems)
            {
                avm.UpdateModel();
            }
        }
    }
}