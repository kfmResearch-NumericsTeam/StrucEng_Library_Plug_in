using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.NewStep
{
    /// <summary>Main view model to assign steps to entries</summary>
    public class ListNewStepViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;
        public RelayCommand CommandChangeStep { get; }
        public RelayCommand CommandDeleteStep { get; }
        public RelayCommand CommandAddStep { get; }

        public ObservableCollection<NewStepViewModel> StepItems;

        private NewStepViewModel _selectedStepItem;

        public NewStepViewModel SelectedStepItem
        {
            get => _selectedStepItem;
            set
            {
                _selectedStepItem = value;
                OnPropertyChanged();
            }
        }

        public ListNewStepViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            StepItems = new ObservableCollection<NewStepViewModel>() { };

            CommandDeleteStep = new RelayCommand(OnDeleteStep);
            CommandChangeStep = new RelayCommand(OnChangeStep);
            CommandAddStep = new RelayCommand(OnAddStep);

            // UpdateVm();

            // _mainVm.ListLayerVm.Layers.CollectionChanged += (sender, args) =>
            // {
            //     AddLayers(args.NewItems);
            //     RemoveLayers(args.OldItems);
            //     ForceRedraw();
            // };
            // _mainVm.ListLoadVm.Loads.CollectionChanged += (sender, args) =>
            // {
            //     AddLoads(args.NewItems);
            //     RemoveLoads(args.OldItems);
            //     ForceRedraw();
            // };
            //
            // ForceRedraw();
        }


        private void UpdateVm()
        {
            foreach (var s in _mainVm.Workbench?.Steps)
            {
                // StepManager.ExistingAggregateStep(s);
            }
        }

        private void ForceRedraw()
        {
        }


        public override void UpdateModel()
        {
            // _mainVm.Workbench.Steps.Clear();
            // foreach (var m in StepManager.ExportModel())
            // {
            //     _mainVm.Workbench.Steps.Add(m);
            // }
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

                var vm = new NewStepViewModel(step)
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