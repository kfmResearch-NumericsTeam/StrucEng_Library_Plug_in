using StrucEngLib.Model.Sm;

namespace StrucEngLib
{
    /// <summary></summary>
    public class SmAdditionalPropertyViewModel : ViewModelBase
    {
        public SandwichProperty Model { get; }

        private string _dStrichBot;

        public string DStrichBot
        {
            get => _dStrichBot;
            set
            {
                _dStrichBot = value;
                OnPropertyChanged();
            }
        }

        private string _dStrichTop;

        public string DStrichTop
        {
            get => _dStrichTop;
            set
            {
                _dStrichTop = value;
                OnPropertyChanged();
            }
        }

        private string _fcK;

        public string FcK
        {
            get => _fcK;
            set
            {
                _fcK = value;
                OnPropertyChanged();
            }
        }

        private string _fcThetaGradKern;

        public string FcThetaGradKern
        {
            get => _fcThetaGradKern;
            set
            {
                _fcThetaGradKern = value;
                OnPropertyChanged();
            }
        }

        private string _fsD;

        public string FsD
        {
            get => _fsD;
            set
            {
                _fsD = value;
                OnPropertyChanged();
            }
        }

        private string _alphaBot;

        public string AlphaBot
        {
            get => _alphaBot;
            set
            {
                _alphaBot = value;
                OnPropertyChanged();
            }
        }

        private string _betaBot;

        public string BetaBot
        {
            get => _betaBot;
            set
            {
                _betaBot = value;
                OnPropertyChanged();
            }
        }

        private string _alphaTop;

        public string AlphaTop
        {
            get => _alphaTop;
            set
            {
                _alphaTop = value;
                OnPropertyChanged();
            }
        }

        private string _betaTop;

        public string BetaTop
        {
            get => _betaTop;
            set
            {
                _betaTop = value;
                OnPropertyChanged();
            }
        }

        public SmAdditionalPropertyViewModel(SmMainViewModel vm, SandwichProperty model)
        {
            Model = model;
            UpdateViewModel();
        }

        public override void UpdateModel()
        {
            Model.DStrichBot = DStrichBot;
            Model.DStrichTop = DStrichTop;
            Model.FcK = FcK;
            Model.FcThetaGradKern = FcThetaGradKern;
            Model.FsD = FsD;
            Model.AlphaBot = AlphaBot;
            Model.BetaBot = BetaBot;
            Model.AlphaTop = AlphaTop;
            Model.BetaTop = BetaTop;
        }

        public sealed override void UpdateViewModel()
        {
            DStrichBot = Model.DStrichBot;
            DStrichTop = Model.DStrichTop;
            FcK = Model.FcK;
            FcThetaGradKern = Model.FcThetaGradKern;
            FsD = Model.FsD;
            AlphaBot = Model.AlphaBot;
            BetaBot = Model.BetaBot;
            AlphaTop = Model.AlphaTop;
            BetaTop = Model.BetaTop;
        }
    }
}