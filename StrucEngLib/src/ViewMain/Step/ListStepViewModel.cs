using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Rhino.Render.ChangeQueue;
using StrucEngLib.Model;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Step
{
    /// <summary>View Model to select ordering of steps for Load processing.</summary>
    public class ListStepViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;
        private readonly ListLayerViewModel _listLayerVm;
        private readonly ListLoadViewModel _listLoadVm;

        private Workbench Model
        {
            get => _listLayerVm.Model;
        }

        // Force Ui to redraw steps
        public event EventHandler RedrawEventHandler;

        private Dictionary<object, SingleStepViewModel> _stepMap; // Key: (Set/Load), Val: ViewModel for single step
        private ObservableCollection<SingleStepViewModel> _steps;

        public ObservableCollection<SingleStepViewModel> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
                OnPropertyChanged();
            }
        }

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
            _listLayerVm = mainVm.ListLayerVm;
            _listLoadVm = mainVm.ListLoadVm;

            Steps = new ObservableCollection<SingleStepViewModel>();
            _stepMap = new Dictionary<object, SingleStepViewModel>();

            _listLoadVm.LoadSettingsChanged += (sender, args) => BuildStepData();
            _listLayerVm.Layers.CollectionChanged += StepDataChanged;
            Steps.CollectionChanged += (sender, args) => HasSteps = Steps.Count != 0;

            BuildStepData();
        }

        private void StepDataChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            BuildStepData();
        }

        private void BuildStepData()
        {
            // XXX: Since we use an event based approach and always update the steps on a change,
            // we have to find out what changed and update the step view accordingly.
            // This makes the below logic tedious, we have to find what's new and what was deleted,
            // while keeping the steps which are backed by an existing layer/ set.
            // In order to keep UI simple, we always redraw view if something changes: RedrawEventHandler
            var stepsToDelete = new Dictionary<object, SingleStepViewModel>(_stepMap);
            int order = HighestOrder() + 1;
            
            foreach (var load in _listLoadVm.Loads)
            {
                if (_stepMap.ContainsKey(load))
                {
                    // Load already backed by a step
                    stepsToDelete.Remove(load);
                    continue;
                }

                Model.Step s = new Model.Step(StepType.Load);
                s.Load = load;
                s.Order = order.ToString();
                order++;
                var vm = new SingleStepViewModel(s);
                Steps.Add(vm);
                _stepMap.Add(load, vm);
                Model.Steps.Add(s);
            }

            foreach (var layer in _listLayerVm.Layers)
            {
                if (layer.LayerType == LayerType.SET)
                {
                    if (_stepMap.ContainsKey(layer))
                    {
                        // Set already backed by a step
                        stepsToDelete.Remove(layer);
                        continue;
                    }

                    Model.Step s = new Model.Step(StepType.Set);
                    s.Set = (Set) layer;
                    s.Order = order.ToString();
                    order++;
                    var vm = new SingleStepViewModel(s);
                    Steps.Add(vm);
                    _stepMap.Add(layer, vm);
                    Model.Steps.Add(s);
                }
            }

            // Remove old steps not backed by data
            foreach (var stepToDelete in stepsToDelete)
            {
                _stepMap.Remove(stepToDelete.Key);
                Steps.Remove(stepToDelete.Value);
                Model.Steps.Remove(stepToDelete.Value.StepModel);
            }

            RedrawEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public int HighestOrder()
        {
            var maxNum = 0;
            foreach (var s in Steps)
            {
                try
                {
                    maxNum = Math.Max(int.Parse(s.Order), maxNum);
                }
                catch (Exception)
                {
                    // Ignore non numeric numbers
                }
            }

            return maxNum;
        }
    }
}