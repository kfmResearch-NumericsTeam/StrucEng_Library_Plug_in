using System;
using System.ComponentModel;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// View model for materials
    /// </summary>
    public class LayerMaterialViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;

        private String _e;

        public String E
        {
            get => _e;
            set
            {
                _e = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _v;

        public String V
        {
            get => _v;
            set
            {
                _v = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private String _p;

        public String P
        {
            get => _p;
            set
            {
                _p = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private Layer GetLayer() => _listLayerVm.SelectedLayer;

        public LayerMaterialViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
            StoreModelToVm();
        }


        private bool _ignoreStoreVmToModel = false;

        private void StoreVmToModel()
        {
            if (_ignoreStoreVmToModel) return;
            var e = (Element) GetLayer();
            if (e.ElementMaterialElastic == null)
            {
                e.ElementMaterialElastic = new ElementMaterialElastic();
            }

            e.ElementMaterialElastic.E = E;
            e.ElementMaterialElastic.V = V;
            e.ElementMaterialElastic.P = P;
        }

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            Element el = (Element) GetLayer();
            if (el.ElementMaterialElastic != null)
            {
                E = el.ElementMaterialElastic.E;
                V = el.ElementMaterialElastic.V;
                P = el.ElementMaterialElastic.P;
            }

            _ignoreStoreVmToModel = false;
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLayerVm.SelectedLayer))
            {
                if (_listLayerVm.SelectedLayer != null && _listLayerVm.SelectedLayer.LayerType == LayerType.ELEMENT)
                {
                    StoreModelToVm();
                }
            }
        }
    }
}