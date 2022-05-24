using System;
using System.ComponentModel;
using System.Text;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// Vm for General Displacement attributes
    /// On each change, we sync with model
    /// </summary>
    public class SetGeneralDisplacementViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;

        private String _ux;

        public String Ux
        {
            get => _ux;
            set
            {
                _ux = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _uy;

        public String Uy
        {
            get => _uy;
            set
            {
                _uy = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _uz;

        public String Uz
        {
            get => _uz;
            set
            {
                _uz = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _rotx;

        public String Rotx
        {
            get => _rotx;
            set
            {
                _rotx = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _roty;

        public String Roty
        {
            get => _roty;
            set
            {
                _roty = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _rotz;

        public String Rotz
        {
            get => _rotz;
            set
            {
                _rotz = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private Layer GetLayer() => _listLayerVm.SelectedLayer;

        private void StoreVmToModel()
        {
            if (_ignoreStoreVmToModel) return;
            var e = (Set) GetLayer();
            if (e.SetGeneralDisplacement == null)
            {
                e.SetGeneralDisplacement = new SetGeneralDisplacement();
            }

            e.SetGeneralDisplacement.Rotx = Rotx;
            e.SetGeneralDisplacement.Rotz = Rotz;
            e.SetGeneralDisplacement.Roty = Roty;
            e.SetGeneralDisplacement.Ux = Ux;
            e.SetGeneralDisplacement.Uy = Uy;
            e.SetGeneralDisplacement.Uz = Uz;
        }

        private bool _ignoreStoreVmToModel = false;

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            var el = (Set) GetLayer();
            if (el.SetGeneralDisplacement != null)
            {
                Rotx = el.SetGeneralDisplacement.Rotx;
                Rotz = el.SetGeneralDisplacement.Rotz;
                Roty = el.SetGeneralDisplacement.Roty;
                Ux = el.SetGeneralDisplacement.Ux;
                Uy = el.SetGeneralDisplacement.Uy;
                Uz = el.SetGeneralDisplacement.Uz;
            }

            _ignoreStoreVmToModel = false;
        }

        public SetGeneralDisplacementViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
            StoreModelToVm();
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLayerVm.SelectedLayer))
            {
                if (_listLayerVm.SelectedLayer != null && _listLayerVm.SelectedLayer.LayerType == LayerType.SET)
                {
                    StoreModelToVm();
                }
            }
        }
    }
}