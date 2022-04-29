using System;
using System.ComponentModel;
using CodeGenerator.Model;

namespace CodeGenerator
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
            if (e.MaterialElastic == null)
            {
                e.MaterialElastic = new MaterialElastic();
            }

            e.MaterialElastic.E = E;
            e.MaterialElastic.V = V;
            e.MaterialElastic.P = P;
        }

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            Element el = (Element) GetLayer();
            if (el.MaterialElastic != null)
            {
                E = el.MaterialElastic.E;
                V = el.MaterialElastic.V;
                P = el.MaterialElastic.P;
            }

            _ignoreStoreVmToModel = false;
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLayerVm.SelectedLayer))
            {
                if (_listLayerVm.SelectedLayer != null && _listLayerVm.SelectedLayer.GetType() == LayerType.ELEMENT)
                {
                    StoreModelToVm();
                }
            }
        }
    }
}