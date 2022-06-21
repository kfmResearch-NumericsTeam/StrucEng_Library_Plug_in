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

        private string _code;

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
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

        private bool _asXiBot = true;

        public bool AsXiBot
        {
            get => _asXiBot;
            set
            {
                _asXiBot = value;
                OnPropertyChanged();
            }
        }

        private bool _asXiTop = true;


        public bool AsXiTop
        {
            get => _asXiTop;
            set
            {
                _asXiTop = value;
                OnPropertyChanged();
            }
        }

        private bool _asEtaBot = true;

        public bool AsEtaBot
        {
            get => _asEtaBot;
            set
            {
                _asEtaBot = value;
                OnPropertyChanged();
            }
        }

        private bool _asEtaTop = true;

        public bool AsEtaTop
        {
            get => _asEtaTop;
            set
            {
                _asEtaTop = value;
                OnPropertyChanged();
            }
        }

        private bool _asZ = true;

        public bool AsZ
        {
            get => _asZ;
            set
            {
                _asZ = value;
                OnPropertyChanged();
            }
        }

        private bool _cCBot = true;

        public bool CCBot
        {
            get => _cCBot;
            set
            {
                _cCBot = value;
                OnPropertyChanged();
            }
        }

        private bool _ccTop = true;

        public bool CCTop
        {
            get => _ccTop;
            set
            {
                _ccTop = value;
                OnPropertyChanged();
            }
        }

        private bool _kBot = true;

        public bool KBot
        {
            get => _kBot;
            set
            {
                _kBot = value;
                OnPropertyChanged();
            }
        }

        private bool _kTop = true;

        public bool KTop
        {
            get => _kTop;
            set
            {
                _kTop = value;
                OnPropertyChanged();
            }
        }

        private bool _TBot = true;

        public bool TBot
        {
            get => _TBot;
            set
            {
                _TBot = value;
                OnPropertyChanged();
            }
        }

        private bool _TTop = true;

        public bool TTop
        {
            get => _TTop;
            set
            {
                _TTop = value;
                OnPropertyChanged();
            }
        }

        private bool _psiBot = true;

        public bool PsiBot
        {
            get => _psiBot;
            set
            {
                _psiBot = value;
                OnPropertyChanged();
            }
        }

        private bool _psiTop = true;

        public bool PsiTop
        {
            get => _psiTop;
            set
            {
                _psiTop = value;
                OnPropertyChanged();
            }
        }

        private bool _fallBot = true;

        public bool FallBot
        {
            get => _fallBot;
            set
            {
                _fallBot = value;
                OnPropertyChanged();
            }
        }

        private bool _fallTop = true;

        public bool FallTop
        {
            get => _fallTop;
            set
            {
                _fallTop = value;
                OnPropertyChanged();
            }
        }

        private bool _mCcBot = true;

        public bool MCcBot
        {
            get => _mCcBot;
            set
            {
                _mCcBot = value;
                OnPropertyChanged();
            }
        }

        private bool _mCcTop = true;

        public bool MCcTop
        {
            get => _mCcTop;
            set
            {
                _mCcTop = value;
                OnPropertyChanged();
            }
        }

        private bool _mShearC = true;

        public bool MShearC
        {
            get => _mShearC;
            set
            {
                _mShearC = value;
                OnPropertyChanged();
            }
        }

        private bool _mcTotal = true;

        public bool MCTotal
        {
            get => _mcTotal;
            set
            {
                _mcTotal = value;
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
                Schubnachweis = m.Schubnachweis;
                Code = m.Code;

                AsXiBot = m.AsXiBot;
                AsXiTop = m.AsXiTop;
                AsEtaBot = m.AsEtaBot;
                AsEtaTop = m.AsEtaTop;
                AsZ = m.AsZ;
                CCBot = m.CCBot;
                CCTop = m.CCTop;
                KBot = m.KBot;
                KTop = m.KTop;
                TBot = m.TBot;
                TTop = m.TTop;
                PsiBot = m.PsiBot;
                PsiTop = m.PsiTop;
                FallBot = m.FallBot;
                FallTop = m.FallTop;
                MCcBot = m.MCcBot;
                MCcTop = m.MCcTop;
                MShearC = m.MShearC;
                MCTotal = m.MCTotal;


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
            m.AxesScale = AxesScale;
            m.DruckzonenIteration = Druckzoneniteration;
            m.MindestBewehrung = Mindestbewehrung;
            m.StepName = SelectedStepName;
            m.Schubnachweis = Schubnachweis;
            m.AdditionalProperties.Clear();
            Properties.ToList().ForEach(p =>
            {
                p.UpdateModel();
                m.AdditionalProperties.Add(p.Model);
            });
            m.Code = Code;
            m.AsXiBot = AsXiBot;
            m.AsXiTop = AsXiTop;
            m.AsEtaBot = AsEtaBot;
            m.AsEtaTop = AsEtaTop;
            m.AsZ = AsZ;
            m.CCBot = CCBot;
            m.CCTop = CCTop;
            m.KBot = KBot;
            m.KTop = KTop;
            m.TBot = TBot;
            m.TTop = TTop;
            m.PsiBot = PsiBot;
            m.PsiTop = PsiTop;
            m.FallBot = FallBot;
            m.FallTop = FallTop;
            m.MCcBot = MCcBot;
            m.MCcTop = MCcTop;
            m.MShearC = MShearC;
            m.MCTotal = MCTotal;
        }
    }
}