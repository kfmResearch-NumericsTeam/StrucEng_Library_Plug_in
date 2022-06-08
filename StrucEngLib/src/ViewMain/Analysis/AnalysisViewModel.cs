using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eto.Forms;
using StrucEngLib.Model;
using StrucEngLib.NewStep;
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

        public AnalysisViewModel(MainViewModel vm)
        {
            if (vm.ListStepVm == null)
            {
                throw new Exception("ListStepVm must be created first");
            }

            _vm = vm;
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();
            UpdateViewModel();
            vm.ListStepVm.StepItems.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null && args.NewItems.Count > 0)
                {
                    foreach (NewStepViewModel stepVm in args.NewItems)
                    {
                        OnNewStep(stepVm);
                    }
                }

                if (args.OldItems != null && args.OldItems.Count > 0)
                {
                    foreach (NewStepViewModel stepVm in args.OldItems)
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
                    if (_selectedItem.Include == false)
                    {
                        _selectedItem.init();
                    }

                    OnPropertyChanged(nameof(SelectedItemVisible));
                }
            };
        }

        private void OnNewStep(NewStepViewModel stepVm)
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

        private void OnRemoveStep(NewStepViewModel stepVm)
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
    }
}