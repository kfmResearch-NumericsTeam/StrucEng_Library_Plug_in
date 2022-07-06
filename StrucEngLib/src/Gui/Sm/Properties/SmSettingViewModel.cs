using System;
using System.Collections.ObjectModel;
using System.Linq;
using Rhino;
using StrucEngLib.Model.Sm;

namespace StrucEngLib.Sm
{
    /// <summary>Vm for general settings of sandwich model</summary>
    public class SmSettingViewModel : ViewModelBase
    {
        private readonly SmMainViewModel _vm;

        public ObservableCollection<SmAdditionalPropertyViewModel> Properties { get; private set; } =
            new ObservableCollection<SmAdditionalPropertyViewModel>();

        public event EventHandler ViewModelInitialized;

        private bool _noStepsAdded = true;

        public bool NoStepsAdded
        {
            get => _noStepsAdded;
            set
            {
                _noStepsAdded = value;
                OnPropertyChanged();
            }
        }

        public bool HasLayers
        {
            get => Properties.Count > 0;
        }

        public void UpdateLayerVisibility()
        {
            OnPropertyChanged(nameof(HasLayers));
            OnPropertyChanged(nameof(HasNoLayers));
        }

        public bool HasNoLayers
        {
            get => !HasLayers;
        }

        private SmAdditionalPropertyViewModel _selectedProperty;

        public SmAdditionalPropertyViewModel SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                UpdateLayerVisibility();
                _selectedProperty = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<string> StepNames = new ObservableCollection<string>();

        private string _selectedStepName;

        public string SelectedStepName
        {
            get => _selectedStepName;
            set
            {
                _selectedStepName = value;
                OnPropertyChanged();
            }
        }


        public bool ContainsLayer(Model.Layer l, ObservableCollection<SmAdditionalPropertyViewModel> props)
        {
            foreach (var vm in props)
            {
                if (vm.Model.Layer == l)
                {
                    return true;
                }
            }

            return false;
        }


        public SmSettingViewModel(SmMainViewModel vm)
        {
            _vm = vm;
            UpdateViewModel();
        }

        public sealed override void UpdateViewModel()
        {
            NoStepsAdded = true;
            StepNames.Clear();
            _vm.Workbench.Steps.ForEach(s =>
            {
                if (s.Setting?.Include == true)
                {
                    NoStepsAdded = false;
                    StepNames.Add(s.Order);
                }
            });
            Properties.Clear();
            if (_vm.Workbench.SandwichModel != null)
            {
                _vm.Workbench.SandwichModel.AdditionalProperties.ForEach(p =>
                {
                    // XXX: Only add properties to layer whose data is also present in workbench.Layers
                    if (_vm.Workbench.Layers.Contains(p.Layer))
                    {
                        var vm = new SmAdditionalPropertyViewModel(_vm, p);
                        Properties.Add(vm);
                    }
                });

                _vm.Workbench.Elements().ForEach(element =>
                {
                    if (!ContainsLayer(element, Properties))
                    {
                        var vm = new SmAdditionalPropertyViewModel(_vm, new SandwichProperty()
                        {
                            Layer = element
                        });
                        Properties.Add(vm);
                    }
                });
            }

            UpdateLayerVisibility();
            ViewModelInitialized?.Invoke(this, new EventArgs());
        }

        public override void UpdateModel()
        {
            if (_vm.Workbench.SandwichModel == null)
            {
                _vm.Workbench.SandwichModel = new SandwichModel();
            }

            var m = _vm.Workbench.SandwichModel;
            m.AdditionalProperties.Clear();
            Properties.ToList().ForEach(p =>
            {
                p.UpdateModel();
                m.AdditionalProperties.Add(p.Model);
            });
        }
    }
}