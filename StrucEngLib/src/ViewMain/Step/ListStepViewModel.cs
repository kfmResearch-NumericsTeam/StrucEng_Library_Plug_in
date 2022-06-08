using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
namespace StrucEngLib.Step
{
    /// <summary>Main view model to assign steps to entries</summary>
    public class ListStepViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;
        public RelayCommand CommandChangeStep { get; }
        public RelayCommand CommandDeleteStep { get; }
        public RelayCommand CommandAddStep { get; }

        public ObservableCollection<StepEntryViewModel> StepItems;

        private StepEntryViewModel _selectedStepItem;

        public StepEntryViewModel SelectedStepItem
        {
            get => _selectedStepItem;
            set
            {
                _selectedStepItem = value;
                OnPropertyChanged();
            }
        }

        public ListStepViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            StepItems = new ObservableCollection<StepEntryViewModel>() { };
            UpdateVm();

            CommandDeleteStep = new RelayCommand(OnDeleteStep);
            CommandChangeStep = new RelayCommand(OnChangeStep);
            CommandAddStep = new RelayCommand(OnAddStep);


            // XXX: Redraw view model if something changes
            _mainVm.ListLayerVm.Layers.CollectionChanged += (sender, args) =>
            {
                foreach (var item in StepItems)
                {
                    item.ModelUpdated();
                }
            };
            _mainVm.ListLoadVm.Loads.CollectionChanged += (sender, args) =>
            {
                foreach (var item in StepItems)
                {
                    item.ModelUpdated();
                }
            };

            if (StepItems.Count > 0)
            {
                // XXX: Preselect first entry
                SelectedStepItem = StepItems[0];
            }
        }

        private void UpdateVm()
        {
            StepItems.Clear();
            if (_mainVm.Workbench.Steps != null)
            {
                foreach (var step in _mainVm.Workbench?.Steps)
                {
                    var sVm = new StepEntryViewModel(step)
                    {
                        Order = step.Order
                    };
                    StepItems.Add(sVm);
                }
            }
        }

        public override void UpdateModel()
        {
            _mainVm.Workbench.Steps.Clear();
            foreach (var m in StepItems)
            {
                _mainVm.Workbench.Steps.Add(m.Model);
            }
        }

        public void OnDeleteStep()
        {
            if (SelectedStepItem == null)
            {
                return;
            }

            StepItems.Remove(SelectedStepItem);
        }

        public void OnChangeStep()
        {
            if (SelectedStepItem == null)
            {
                return;
            }

            var selection = GetSelectionListToAdd();
            var dialog = new AddStepView(SelectedStepItem.Order, selection);
            var rc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (rc == DialogResult.Ok)
            {
                var selected = dialog.SelectedEntries;
                var vm = SelectedStepItem;
                vm.Model.Entries.Clear();
                foreach (var e in dialog.SelectedEntries)
                {
                    vm.Model.Entries.Add(e);
                }

                vm.ModelUpdated();
            }
        }

        public void OnAddStep()
        {
            var selection = GetSelectionListToAdd();
            var stepOrder = NewStepOrder();

            var dialog = new AddStepView(stepOrder, selection);
            var rc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (rc == DialogResult.Ok)
            {
                var selected = dialog.SelectedEntries;
                if (selected == null || selected.Count == 0)
                {
                    // XXX: Nothing to add
                    return;
                }

                var step = new Model.Step();
                foreach (var e in dialog.SelectedEntries)
                {
                    step.Entries.Add(e);
                }

                var vm = new StepEntryViewModel(step)
                {
                    Order = stepOrder
                };
                StepItems.Add(vm);
            }
        }

        public List<KeyValuePair<string, StepEntry>> GetSelectionListToAdd()
        {
            var entries = new List<KeyValuePair<string, StepEntry>>();
            if (_mainVm.ListLayerVm.Layers != null)
            {
                foreach (Layer l in _mainVm.ListLayerVm.Layers)
                {
                    if (l.LayerType == LayerType.SET)
                    {
                        var newEntry = new StepEntry(StepType.Set, l);
                        var desc = "Set: " + l.GetName();
                        entries.Add(new KeyValuePair<string, StepEntry>(desc, newEntry));
                    }
                }
            }

            if (_mainVm.ListLoadVm.Loads != null)
            {
                foreach (Load l in _mainVm.ListLoadVm.Loads)
                {
                    var newEntry = new StepEntry(StepType.Load, l);
                    var desc = "Load: " + l.Description;
                    entries.Add(new KeyValuePair<string, StepEntry>(desc, newEntry));
                }
            }

            return entries;
        }

        public string NewStepOrder()
        {
            int order = 0;
            foreach (var stepVm in StepItems)
            {
                try
                {
                    order = Math.Max(order, int.Parse(stepVm.Order));
                }
                catch (Exception e)
                {
                    // Ignore
                }
            }

            order++;
            return order.ToString();
        }
    }
}