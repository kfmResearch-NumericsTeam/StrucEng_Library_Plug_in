using StrucEngLib.Model.Sm;

namespace StrucEngLib.Gui.Sm
{
    /// <summary>Vm for a single analysis item</summary>
    public class SmAnalysisItemViewModel : ViewModelBase
    {
        public SmAnalysisSetting Model { get; }

        public string StepName => Model.Step.Order;

        private bool _include;

        public bool Include
        {
            get => _include;
            set
            {
                _include = value;
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

        private bool _tBot = true;

        public bool TBot
        {
            get => _tBot;
            set
            {
                _tBot = value;
                OnPropertyChanged();
            }
        }

        private bool _tTop = true;

        public bool TTop
        {
            get => _tTop;
            set
            {
                _tTop = value;
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


        public SmAnalysisItemViewModel(SmAnalysisSetting model)
        {
            Model = model;
            Init();
            ModelToVm(model, this);
        }

        public static SmAnalysisItemViewModel CreateNew(SmAnalysisSetting model)
        {
            SmAnalysisItemViewModel vm = new SmAnalysisItemViewModel(model);
            vm.Init();
            vm.UpdateModel();
            return vm;
        }

        public override void UpdateModel()
        {
            VmToModel(this, Model);
        }

        public override void UpdateViewModel()
        {
            ModelToVm(Model, this);
        }

        public static void ModelToVm(SmAnalysisSetting m, SmAnalysisItemViewModel vm)
        {
            vm.Include = m.Include;
            vm.AxesScale = m.AxesScale;
            vm.Druckzoneniteration = m.DruckzonenIteration;
            vm.Mindestbewehrung = m.MindestBewehrung;
            vm.Schubnachweis = m.Schubnachweis;
            vm.Code = m.Code;

            vm.AsXiBot = m.AsXiBot;
            vm.AsXiTop = m.AsXiTop;
            vm.AsEtaBot = m.AsEtaBot;
            vm.AsEtaTop = m.AsEtaTop;
            vm.AsZ = m.AsZ;
            vm.CCBot = m.CCBot;
            vm.CCTop = m.CCTop;
            vm.KBot = m.KBot;
            vm.KTop = m.KTop;
            vm.TBot = m.TBot;
            vm.TTop = m.TTop;
            vm.PsiBot = m.PsiBot;
            vm.PsiTop = m.PsiTop;
            vm.FallBot = m.FallBot;
            vm.FallTop = m.FallTop;
            vm.MCcBot = m.MCcBot;
            vm.MCcTop = m.MCcTop;
            vm.MShearC = m.MShearC;
            vm.MCTotal = m.MCTotal;
        }

        public static void VmToModel(SmAnalysisItemViewModel v, SmAnalysisSetting m)
        {
            m.Include = v.Include;

            m.AxesScale = v.AxesScale;
            m.DruckzonenIteration = v.Druckzoneniteration;
            m.MindestBewehrung = v.Mindestbewehrung;
            m.Schubnachweis = v.Schubnachweis;
            m.Code = v.Code;
            m.AsXiBot = v.AsXiBot;
            m.AsXiTop = v.AsXiTop;
            m.AsEtaBot = v.AsEtaBot;
            m.AsEtaTop = v.AsEtaTop;
            m.AsZ = v.AsZ;
            m.CCBot = v.CCBot;
            m.CCTop = v.CCTop;
            m.KBot = v.KBot;
            m.KTop = v.KTop;
            m.TBot = v.TBot;
            m.TTop = v.TTop;
            m.PsiBot = v.PsiBot;
            m.PsiTop = v.PsiTop;
            m.FallBot = v.FallBot;
            m.FallTop = v.FallTop;
            m.MCcBot = v.MCcBot;
            m.MCcTop = v.MCcTop;
            m.MShearC = v.MShearC;
            m.MCTotal = v.MCTotal;
        }

        public void Init()
        {
            Include = false;
            AxesScale = "";
            Druckzoneniteration = "true";
            Mindestbewehrung = "true";
            Schubnachweis = "";
            Code = "";

            AsXiBot = true;
            AsXiTop = true;
            AsEtaBot = true;
            AsEtaTop = true;
            AsZ = true;
            CCBot = true;
            CCTop = true;
            KBot = true;
            KTop = true;
            TBot = true;
            TTop = true;
            PsiBot = true;
            PsiTop = true;
            FallBot = true;
            FallTop = true;
            MCcBot = true;
            MCcTop = true;
            MShearC = true;
            MCTotal = true;
        }
    }
}