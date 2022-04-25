using System;
using System.Collections.Generic;
using System.ComponentModel;
using CodeGenerator.Model;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class LayerDetailsViewModel : ViewModelBase
    {
        // private SectionViewModel _elementSectionVm;
        // private SectionViewModel _elementMaterialVm;
        // private SectionViewModel _setDisplacementVm;

        // private LayerDisplacementViewModel _setDisplacementVm;
        // private LayerMaterialViewModel _materialVm;
        // private LayerSectionViewModel _sectionVm;

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
                Rhino.RhinoApp.WriteLine("_layerDetailView changed");
            }
        }

        private readonly ListLayerViewModel _listLayerVm;

        public LayerDetailsViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            _listLayerVm.PropertyChanged += ListLayerVmOnPropertyChanged;
        }

        private void ListLayerVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLayerVm.SelectedLayer))
            {
                LayerDetailViewVisible = _listLayerVm.SelectedLayer != null;
                if (_listLayerVm.SelectedLayer == null)
                {
                }
                else
                {
                    Control c = GetPropertyContentForLayer(_listLayerVm.SelectedLayer);
                    LayerDetailView = c;
                }
            }
        }

        private Control _GetPropertyContentForElement(Element e)
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            
            var _sectionViewModel = new LayerSectionViewModel(_listLayerVm);
            layout.Rows.Add(new LayerSectionView(_sectionViewModel));
            
            var _elementMaterialVm = new LayerMaterialViewModel(_listLayerVm);
            layout.Rows.Add(new LayerMaterialView(_elementMaterialVm));

            return layout;
        }

        private Control _GetPropertyContentForSet(Set s)
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            var _setDisplacementVm = new LayerDisplacementViewModel(_listLayerVm);
            layout.Rows.Add(new LayerDisplacementView(_setDisplacementVm));
            return layout;
        }

        public Control GetPropertyContentForLayer(Layer e)
        {
            if (e.GetType() == LayerType.ELEMENT)
            {
                return _GetPropertyContentForElement((Element) e);
            }
            else if (e.GetType() == LayerType.SET)
            {
                return _GetPropertyContentForSet((Set) e);
            }
            else
            {
                throw new SystemException("Unknown layer type");
            }
        }
    }
}