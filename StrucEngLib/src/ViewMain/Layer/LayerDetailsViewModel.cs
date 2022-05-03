using System;
using System.Collections.Generic;
using System.ComponentModel;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// LayerDetailsViewModel builds subviews which show detail information for a layer.
    /// </summary>
    public class LayerDetailsViewModel : ViewModelBase
    {
        private bool _layerDetailViewVisible;

        public bool LayerDetailViewVisible
        {
            get => _layerDetailViewVisible;
            set
            {
                if (_layerDetailViewVisible == value) return;
                _layerDetailViewVisible = value;
                OnPropertyChanged();
            }
        }

        private Control _layerDetailView;

        public Control LayerDetailView
        {
            get => _layerDetailView;
            set
            {
                _layerDetailView = value;
                OnPropertyChanged();
            }
        }

        private readonly ListLayerViewModel _listLayerVm;

        public LayerDetailsViewModel(MainViewModel mainVm)
        {
            _listLayerVm = mainVm.ListLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(_listLayerVm.SelectedLayer)) return;
            LayerDetailViewVisible = _listLayerVm.SelectedLayer != null;
            if (_listLayerVm.SelectedLayer != null)
            {
                Control c = GetPropertyContentForLayer(_listLayerVm.SelectedLayer);
                LayerDetailView = c;
            }
        }

        private Control _GetPropertyContentForElement(Element e)
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            var sectionViewModel = new LayerSectionViewModel(_listLayerVm);
            layout.Rows.Add(new LayerSectionView(sectionViewModel));

            var elementMaterialVm = new LayerMaterialViewModel(_listLayerVm);
            layout.Rows.Add(new LayerMaterialView(elementMaterialVm));

            return layout;
        }

        private Control _GetPropertyContentForSet(Set s)
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            var setDisplacementVm = new LayerDisplacementViewModel(_listLayerVm);
            layout.Rows.Add(new LayerDisplacementView(setDisplacementVm));
            return layout;
        }

        public Control GetPropertyContentForLayer(Layer e)
        {
            if (e.LayerType == LayerType.ELEMENT)
            {
                return _GetPropertyContentForElement((Element) e);
            }

            if (e.LayerType == LayerType.SET)
            {
                return _GetPropertyContentForSet((Set) e);
            }

            throw new SystemException("Unknown layer type");
        }
    }
}