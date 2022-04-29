using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CodeGenerator.Model;
using CodeGenerator.ViewMain.Step;

namespace CodeGenerator.Step
{
    /// <summary>View Model to select ordering of steps for Load processing.</summary>
    public class StepViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;
        private readonly ListLoadViewModel _listLoadVm;

        private Workbench Model
        {
            get => _listLayerVm.Model;
        }

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

        public StepViewModel(ListLayerViewModel listLayerVm, ListLoadViewModel listLoadVm)
        {
            _listLayerVm = listLayerVm;
            _listLoadVm = listLoadVm;
            Steps = new ObservableCollection<SingleStepViewModel>();
            _stepMap = new Dictionary<object, SingleStepViewModel>();
            BuildStepData();

            _listLoadVm.Loads.CollectionChanged += StepDataChanged;
            _listLayerVm.Layers.CollectionChanged += StepDataChanged;
            Steps.CollectionChanged += (sender, args) => HasSteps = Steps.Count != 0;
        }

        private void StepDataChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            BuildStepData();
        }

        private void BuildStepData()
        {
            // XXX: Since we use an event based approach and always update the steps in the view on a change,
            // we have to find out what changed and update the step view accordingly.
            // This makes the below logic tedious, we have to find what's new and what was deleted.
            var stepsToDelete = new Dictionary<object, SingleStepViewModel>(_stepMap);
            int order = HighestOrder() + 1;

            foreach (var load in _listLoadVm.Loads)
            {
                if (_stepMap.ContainsKey(load))
                {
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
            }

            foreach (var layer in _listLayerVm.Layers)
            {
                if (layer.LayerType == LayerType.SET)
                {
                    if (_stepMap.ContainsKey(layer))
                    {
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

            foreach (var stepToDelete in stepsToDelete)
            {
                _stepMap.Remove(stepToDelete.Key);
                Steps.Remove(stepToDelete.Value);
                Model.Steps.Remove(stepToDelete.Value.StepModel);
            }
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