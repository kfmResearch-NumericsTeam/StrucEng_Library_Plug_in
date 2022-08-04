using System;
using System.Collections.ObjectModel;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib.Analysis
{
    /// <summary>VM to attach settings to a step. For each aggregated step we create a settings object</summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly LinFeMainViewModel _vm;

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


        public AnalysisViewModel(LinFeMainViewModel vm)
        {
            if (vm.ListStepVm == null)
            {
                throw new Exception("ListStepVm must be created first");
            }

            _vm = vm;
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();
            UpdateViewModel();
            AddNewAndRemoveOldSteps(vm);
            vm.ListStepVm.StepChanged += (sender, args) =>
            {
                foreach (var avm in AnalysisViewItems)
                {
                    // XXX: If step contains a set, we cant generate analysis output for it.
                    // So we reset it to contain no user data.
                    if (!avm.HasOutput() && avm.Include == true)
                    {
                        avm.init();
                        avm.Include = true;
                    }
                }

                // XXX: Force to redraw output (workaround, TODO)
                var item = SelectedItem;
                SelectedItem = null;
                SelectedItem = item;
            };
        }

        private void AddNewAndRemoveOldSteps(LinFeMainViewModel vm)
        {
            vm.ListStepVm.StepItems.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null && args.NewItems.Count > 0)
                {
                    foreach (StepEntryViewModel stepVm in args.NewItems)
                    {
                        OnNewStep(stepVm);
                    }
                }

                if (args.OldItems != null && args.OldItems.Count > 0)
                {
                    foreach (StepEntryViewModel stepVm in args.OldItems)
                    {
                        OnRemoveStep(stepVm);
                    }
                }
            };
        }

        private void RegisterEvents(AnalysisItemViewModel vm)
        {
            // XXX: We update SelectedItemVisible if include flag is changed
            vm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_selectedItem.Include))
                {
                    if (_selectedItem != null && _selectedItem.Include == false)
                    {
                        _selectedItem.init();
                    }

                    OnPropertyChanged(nameof(SelectedItemVisible));
                }
            };
        }

        private void OnNewStep(StepEntryViewModel stepVm)
        {
            if (stepVm.Model.Setting == null)
            {
                stepVm.Model.Setting = new AnalysisSetting();
            }

            var avm = new AnalysisItemViewModel(stepVm.Model)
            {
                StepName = stepVm.Order
            };
            AnalysisViewItems.Add(avm);
            RegisterEvents(avm);
        }

        private void OnRemoveStep(StepEntryViewModel stepVm)
        {
            var vm = ViewModelByModel(stepVm.Model);
            if (vm != null)
            {
                AnalysisViewItems.Remove(vm);
            }
        }

        private AnalysisItemViewModel ViewModelByModel(Model.Step model)
        {
            foreach (var vm in AnalysisViewItems)
            {
                if (vm.StepModel == model)
                {
                    return vm;
                }
            }

            return null;
        }


        public override void UpdateModel()
        {
            // XXX: Analysis Setting is part of Step Model
            foreach (var avm in AnalysisViewItems)
            {
                avm.UpdateModel();
            }
        }

        public sealed override void UpdateViewModel()
        {
            AnalysisViewItems.Clear();
            foreach (var step in _vm.ListStepVm.StepItems)
            {
                OnNewStep(step);
            }
        }

        public void RhinoSelectStep()
        {
            if (SelectedItem == null) return;
            RhinoUtils.SelectLayerByNames(RhinoDoc.ActiveDoc, SelectedItem.StepModel.LayerNames());
        }
    }
}