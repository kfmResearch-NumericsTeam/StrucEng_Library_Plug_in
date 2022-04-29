using System;
using System.ComponentModel;
using System.Text;
using CodeGenerator.Model;

namespace CodeGenerator
{
    /// <summary>
    /// Vm for Displacement
    /// </summary>
    public class LayerDisplacementViewModel : ViewModelBase
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
            if (e.Displacement == null)
            {
                e.Displacement = new Displacement();
            }

            e.Displacement.Rotx = Rotx;
            e.Displacement.Rotz = Rotz;
            e.Displacement.Roty = Roty;
            e.Displacement.Ux = Ux;
            e.Displacement.Uy = Uy;
            e.Displacement.Uz = Uz;
        }

        private bool _ignoreStoreVmToModel = false;

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            var el = (Set) GetLayer();
            if (el.Displacement != null)
            {
                Rotx = el.Displacement.Rotx;
                Rotz = el.Displacement.Rotz;
                Roty = el.Displacement.Roty;
                Ux = el.Displacement.Ux;
                Uy = el.Displacement.Uy;
                Uz = el.Displacement.Uz;
            }

            _ignoreStoreVmToModel = false;
        }

        public LayerDisplacementViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
            StoreModelToVm();
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLayerVm.SelectedLayer))
            {
                if (_listLayerVm.SelectedLayer != null && _listLayerVm.SelectedLayer.GetType() == LayerType.SET)
                {
                    StoreModelToVm();
                }
            }
        }
    }
}