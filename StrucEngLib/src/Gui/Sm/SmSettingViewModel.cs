using System.Collections.ObjectModel;
using System.Linq;
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

        public SmAdditionalPropertyViewModel _selectedProperty;

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
        
        private string _stepName;

        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Steps { get; private set; }

        private bool _mindestbewehrung;

        public string Mindestbewehrung
        {
            get => _mindestbewehrung ? "true" : "false";
            set
            {
                if ("true".Equals(value?.ToLower().Trim()))
                {
                    _mindestbewehrung = true;
                }
                else
                {
                    _mindestbewehrung = false;
                }

                OnPropertyChanged();
            }
        }

        private bool _druckzoneniteration;

        public string Druckzoneniteration
        {
            get => _druckzoneniteration ? "true" : "false";
            set
            {
                if ("true".Equals(value?.ToLower().Trim()))
                {
                    _druckzoneniteration = true;
                }
                else
                {
                    _druckzoneniteration = false;
                }

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
            StepNames.Clear();
            _vm.Workbench.Steps.ForEach(s => { StepNames.Add("Step " + s.Order); });
            Properties.Clear();
            if (_vm.Workbench.SandwichModel != null)
            {
                var m = _vm.Workbench.SandwichModel;
                AxesScale = m.AxesScale;
                Druckzoneniteration = m.DruckzonenIteration;
                Mindestbewehrung = m.MindestBewehrung;
                StepName = m.StepName;
                _vm.Workbench.SandwichModel.AdditionalProperties.ForEach(p =>
                {
                    SmAdditionalPropertyViewModel vm = new SmAdditionalPropertyViewModel(_vm, p);
                    Properties.Add(vm);
                });

                _vm.Workbench.Elements().ForEach(element =>
                {
                    if (!ContainsLayer(element, Properties))
                    {
                        SmAdditionalPropertyViewModel vm = new SmAdditionalPropertyViewModel(_vm, new SandwichProperty()
                        {
                            Layer = element
                        });
                        Properties.Add(vm);
                    }
                });
            }
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
            m.StepName = StepName;
            m.AdditionalProperties.Clear();
            Properties.ToList().ForEach(p =>
            {
                p.UpdateModel();
                m.AdditionalProperties.Add(p.Model);
            });
        }
    }
}