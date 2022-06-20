using System.Collections.ObjectModel;
using System.Linq;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;

namespace StrucEngLib
{
    /// <summary></summary>
    public class SmSettingViewModel : ViewModelBase
    {
        private readonly SmMainViewModel _vm;

        public ObservableCollection<SmAdditionalPropertyViewModel> Properties { get; private set; } =
            new ObservableCollection<SmAdditionalPropertyViewModel>();

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

        private string _mindestbewehrung;

        public string Mindestbewehrung
        {
            get => _mindestbewehrung;
            set
            {
                _mindestbewehrung = value;
                OnPropertyChanged();
            }
        }

        private string _druckzoneniteration;

        public string Druckzoneniteration
        {
            get => _druckzoneniteration;
            set
            {
                _druckzoneniteration = value;
                OnPropertyChanged();
            }
        }

        private string _schubnachweis;

        public string Schubnachweis
        {
            get => _schubnachweis;
            set
            {
                _schubnachweis = value;
                OnPropertyChanged();
            }
        }

        private string _axesScale;

        public string AxesScale
        {
            get => _axesScale;
            set
            {
                _axesScale = value;
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
                var m = _vm.Workbench.SandwichModel;
                AxesScale = m.AxesScale;
                Druckzoneniteration = m.DruckzonenIteration;
                Mindestbewehrung = m.MindestBewehrung;
                SelectedStepName = m.StepName;
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

            OnPropertyChanged(nameof(HasLayers));
            OnPropertyChanged(nameof(HasNoLayers));
        }

        public override void UpdateModel()
        {
            if (_vm.Workbench.SandwichModel == null)
            {
                _vm.Workbench.SandwichModel = new SandwichModel();
            }

            var m = _vm.Workbench.SandwichModel;
            m.AxesScale = AxesScale;
            m.DruckzonenIteration = Druckzoneniteration;
            m.MindestBewehrung = Mindestbewehrung;
            m.StepName = SelectedStepName;
            m.AdditionalProperties.Clear();
            Properties.ToList().ForEach(p =>
            {
                p.UpdateModel();
                m.AdditionalProperties.Add(p.Model);
            });
        }
    }
}