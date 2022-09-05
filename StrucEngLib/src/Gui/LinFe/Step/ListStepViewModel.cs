using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Step
{
    using Layer = StrucEngLib.Model.Layer;
    using Load = StrucEngLib.Model.Load;

    /// <summary>Main view model to assign steps to entries</summary>
    public class ListStepViewModel : ViewModelBase
    {
        private readonly LinFeMainViewModel _mainVm;
        public RelayCommand CommandChangeStep { get; }
        public RelayCommand CommandDeleteStep { get; }
        public RelayCommand CommandAddStep { get; }

        public ObservableCollection<StepEntryViewModel> StepItems;

        private StepEntryViewModel StepbyEntryObject(object entry)
        {
            foreach (var stepItem in StepItems)
            {
                if (stepItem.Model.Contains(entry))
                {
                    return stepItem;
                }
            }

            return null;
        }


        /// <summary>
        /// This EventHandler will fire if a new Step is Added or a property within StepItems is changed. 
        /// </summary>
        public event EventHandler<EventArgs> StepChanged;

        protected void OnStepChanged(Object sender)
        {
            StepChanged?.Invoke(sender, new EventArgs());
        }

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

        private void AddStepItem(StepEntryViewModel step)
        {
            StepItems.Add(step);
            step.PropertyChanged += (sender, args) => { OnStepChanged(step); };
        }

        public ListStepViewModel(LinFeMainViewModel mainVm)
        {
            _mainVm = mainVm;
            StepItems = new ObservableCollection<StepEntryViewModel>() { };
            StepItems.CollectionChanged += (sender, args) => { OnStepChanged(this); };

            UpdateVm();

            CommandDeleteStep = new RelayCommand(OnDeleteStep);
            CommandChangeStep = new RelayCommand(OnChangeStep);
            CommandAddStep = new RelayCommand(OnAddStep);

            InitEventListeners();

            if (StepItems.Count > 0)
            {
                // XXX: Preselect first entry
                SelectedStepItem = StepItems[0];
            }
        }

        private void InitEventListeners()
        {
            // XXX: Redraw view model if something changes
            _mainVm.ListLayerVm.Layers.CollectionChanged += (sender, args) =>
            {
                // In case a layer is removed, we have to remove it from the step
                if (args.OldItems != null)
                {
                    foreach (var removeLayer in args.OldItems)
                    {
                        var stepVm = StepbyEntryObject(removeLayer);
                        stepVm?.Model.RemoveStepEntryWithValue(removeLayer);
                    }
                }

                // Update set item text
                foreach (var item in StepItems)
                {
                    item.ModelUpdated();
                }
            };


            _mainVm.ListLoadVm.Loads.CollectionChanged += (sender, args) =>
            {
                // In case a load is removed, we have to remove it from the step
                if (args.OldItems != null)
                {
                    foreach (var removeLayer in args.OldItems)
                    {
                        var stepVm = StepbyEntryObject(removeLayer);
                        stepVm?.Model.RemoveStepEntryWithValue(removeLayer);
                    }
                }

                foreach (var item in StepItems)
                {
                    item.ModelUpdated();
                }
            };

            // Update load item text
            _mainVm.ListLoadVm.LoadChanged += (sender, args) =>
            {
                foreach (var item in StepItems)
                {
                    item.ModelUpdated();
                }
            };
        }

        private void UpdateVm()
        {
            StepItems.Clear();
            if (_mainVm.Workbench.Steps != null)
            {
                foreach (var step in _mainVm.Workbench?.Steps)
                {
                    var sVm = new StepEntryViewModel(_mainVm, step)
                    {
                        Order = step.Order
                    };
                    AddStepItem(sVm);
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

                var vm = new StepEntryViewModel(this._mainVm, step)
                {
                    Order = stepOrder
                };
                AddStepItem(vm);
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
                        var desc = "Constraint: " + l.GetName();
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
                catch (Exception)
                {
                    // Ignore
                }
            }

            order++;
            return order.ToString();
        }

        public void RhinoSelectStep()
        {
            if (SelectedStepItem == null) return;
            var names = SelectedStepItem.Model.LayerNames();
            if (names.Count == 0)
            {
                RhinoUtils.UnSelectAll(RhinoDoc.ActiveDoc);
            }
            else
            {
                RhinoUtils.SelectLayerByNames(RhinoDoc.ActiveDoc, names.ToList());
            }
        }
    }
}