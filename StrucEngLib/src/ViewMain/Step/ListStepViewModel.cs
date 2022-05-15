using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Rhino;
using Rhino.Render.ChangeQueue;
using Rhino.Runtime.InteropWrappers;
using StrucEngLib.Model;
using StrucEngLib.Utils;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Step
{
    /// <summary>View Model to select ordering of steps.</summary>
    public class ListStepViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;

        public static string StepNameExclude { get; } = "Excluded";

        public ObservableCollection<string> StepNames { get; }

        public event EventHandler RedrawEventHandler;

        public ObservableCollection<SingleStepViewModel> Steps { get; }

        private Dictionary<object, SingleStepViewModel> _stepMap;

        private bool _hasSteps = false;

        public bool HasSteps
        {
            get => _hasSteps;
            set
            {
                _hasSteps = value;
                OnPropertyChanged();
            }
        }

        public ListStepViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            StepNames = new ObservableCollection<string> {StepNameExclude};
            Steps = new ObservableCollection<SingleStepViewModel>();
            _stepMap = new Dictionary<object, SingleStepViewModel>();

            _mainVm.ListLoadVm.LoadSettingsChanged += (sender, args) => ForceRedraw();
            Steps.CollectionChanged += (sender, args) => HasSteps = Steps.Count != 0;
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

            AddLoads(_mainVm.ListLoadVm.Loads);
            AddLayers(_mainVm.ListLayerVm.Layers);
        }

        private void RemoveLayers(IList layers)
        {
            foreach (var oldItem in layers ?? Enumerable.Empty<Load>().ToList())
            {
                var l = (Layer) oldItem;
                if (l.LayerType == LayerType.SET)
                {
                    var vm = _stepMap[l];
                    Steps.Remove(vm);
                    _stepMap.Remove(l);
                }
            }
        }

        private void AddLayers(IList layers)
        {
            int order = HighestOrder() + 1;
            foreach (Layer l in layers ?? Enumerable.Empty<Load>().ToList())
            {
                if (l.LayerType == LayerType.SET)
                {
                    var vm = new SingleStepViewModel(
                        new Model.Step
                        {
                            StepType = StepType.Set,
                            Set = (Set) l,
                            Order = order.ToString()
                        });
                    AddIfNotContains(StepNames, order.ToString());
                    order++;
                    _stepMap[l] = vm;
                    Steps.Add(vm);
                }
            }
        }

        private void RemoveLoads(IList loads)
        {
            foreach (Load l in loads ?? Enumerable.Empty<Load>().ToList())
            {
                var vm = _stepMap[l];
                Steps.Remove(vm);
                _stepMap.Remove(l);
            }
        }

        private void AddLoads(IEnumerable loads)
        {
            int order = HighestOrder() + 1;
            foreach (Load l in loads ?? Enumerable.Empty<Load>().ToList())
            {
                var vm = new SingleStepViewModel(
                    new Model.Step
                    {
                        StepType = StepType.Load,
                        Load = l,
                        Order = order.ToString()
                    });

                AddIfNotContains(StepNames, order.ToString());
                order++;
                _stepMap[l] = vm;
                Steps.Add(vm);
            }
        }

        private int HighestOrder() =>
            Steps.Select(s => s.Order)
                .DefaultIfEmpty("0")
                .Where(order => int.TryParse(order, out _))
                .Select(o => int.Parse(o))
                .Max();

        private void ForceRedraw()
        {
            RedrawEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void AddIfNotContains<T>(ObservableCollection<T> c, T value)
        {
            if (!c.Contains(value))
            {
                c.Add(value);
            }
        }

        public override void UpdateModel()
        {
            _mainVm.Workbench.Steps.Clear();
            foreach (var s in Steps)
            {
                s.UpdateModel();
                _mainVm.Workbench.Steps.Add(s.Model);
            }
        }
    }
}