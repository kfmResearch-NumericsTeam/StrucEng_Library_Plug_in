using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using StrucEngLib.Model;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Step
{
    /// <summary>Main view model to assign steps to entries</summary>
    public class ListStepViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;

        public event EventHandler RedrawEventHandler;

        public StepManager StepManager { get; }

        public ObservableCollection<AggregatedStepViewModel> AggregatedSteps => StepManager.AggregatedSteps;
        public ObservableCollection<StepViewModel> Steps => StepManager.Steps;
        public ObservableCollection<string> StepNames => StepManager.StepNames;

        public ListStepViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            StepManager = new StepManager(mainVm);
            UpdateVm();

            _mainVm.ListLayerVm.Layers.CollectionChanged += (sender, args) =>
            {
                AddLayers(args.NewItems);
                RemoveLayers(args.OldItems);
                ForceRedraw();
            };
            _mainVm.ListLoadVm.Loads.CollectionChanged += (sender, args) =>
            {
                AddLoads(args.NewItems);
                RemoveLoads(args.OldItems);
                ForceRedraw();
            };

            ForceRedraw();
        }

        private void RemoveLayers(IList layers)
        {
            foreach (var oldItem in layers ?? Enumerable.Empty<Load>().ToList())
            {
                var l = (Layer) oldItem;
                if (l.LayerType == LayerType.SET)
                {
                    StepManager.RemoveStep(StepType.Set, l);
                }
            }
        }

        private void AddLayers(IList layers)
        {
            foreach (Layer l in layers ?? Enumerable.Empty<Load>().ToList())
            {
                if (l.LayerType == LayerType.SET)
                {
                    var newModel = new Model.Step();
                    var newEntry = new StepEntry(StepType.Set, l);
                    StepManager.NewAggregationStep(newModel, newEntry);
                }
            }
        }

        private void RemoveLoads(IList loads)
        {
            foreach (Load l in loads ?? Enumerable.Empty<Load>().ToList())
            {
                StepManager.RemoveStep(StepType.Load, l);
            }
        }

        private void AddLoads(IEnumerable loads)
        {
            foreach (Load l in loads ?? Enumerable.Empty<Load>().ToList())
            {
                var newModel = new Model.Step();
                var newEntry = new StepEntry(StepType.Load, l);
                StepManager.NewAggregationStep(newModel, newEntry);
            }
        }

        private void UpdateVm()
        {
            foreach (var s in _mainVm.Workbench?.Steps)
            {
                StepManager.ExistingAggregateStep(s);
            }
        }

        private void ForceRedraw()
        {
            RedrawEventHandler?.Invoke(this, EventArgs.Empty);
        }


        public override void UpdateModel()
        {
            _mainVm.Workbench.Steps.Clear();
            foreach (var m in StepManager.ExportModel())
            {
                _mainVm.Workbench.Steps.Add(m);
            }
        }
    }
}