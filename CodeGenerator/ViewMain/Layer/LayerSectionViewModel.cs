using System;
using System.ComponentModel;
using CodeGenerator.Model;

namespace CodeGenerator
{
    /// <summary>
    /// Vm for Section information
    /// </summary>
    public class LayerSectionViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;

        private string _thickness;

        public string Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                OnPropertyChanged();
                StoreVmToModel();
            }
        }

        private Layer GetLayer() => _listLayerVm.SelectedLayer;

        private bool _ignoreStoreVmToModel = false;

        private void StoreVmToModel()
        {
            if (_ignoreStoreVmToModel) return;
            var e = (Element) GetLayer();
            if (e.ShellSection == null)
            {
                e.ShellSection = new ShellSection();
            }

            e.ShellSection.Thickness = Thickness;
        }

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            var el = (Element) GetLayer();
            if (el.ShellSection != null)
            {
                Thickness = el.ShellSection.Thickness;
            }

            _ignoreStoreVmToModel = false;
        }

        public LayerSectionViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
            StoreModelToVm();
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