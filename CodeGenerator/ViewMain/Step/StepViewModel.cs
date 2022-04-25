using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CodeGenerator.Model;

namespace CodeGenerator.Step
{
    public class SingleStepViewModel : ViewModelBase
    {
        private readonly Model.Step _step;

        private string _order;

        public string Order
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged();
                _step.Order = _order;
            }
        }

        public string Label => _step.getSummary();

        public SingleStepViewModel(Model.Step step)
        {
            _step = step;
        }
    }

    /**
     * View Model to select ordering of steps for Load processing.
     */
    public class StepViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;
        private readonly ListLoadViewModel _listLoadVm;

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

        public StepViewModel(ListLayerViewModel listLayerVm, ListLoadViewModel listLoadVm)
        {
            _listLayerVm = listLayerVm;
            _listLoadVm = listLoadVm;
            Steps = new ObservableCollection<SingleStepViewModel>();

            _listLoadVm.Loads.CollectionChanged += LoadsChanged;
        }

        private void LoadsChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Rhino.RhinoApp.WriteLine("loads changed");
            Steps.Clear();
            foreach (var load in _listLoadVm.Loads)
            {
                Model.Step s = new Model.Step(StepType.Load);
                s.Load = load;
                Steps.Add(new SingleStepViewModel(s));
            }

            foreach (var layer in _listLayerVm.Layers)
            {
                if (layer.GetType() == LayerType.SET)
                {
                    Model.Step s = new Model.Step(StepType.Set);
                    Steps.Add(new SingleStepViewModel(s));
                }
            }
        }
    }
}