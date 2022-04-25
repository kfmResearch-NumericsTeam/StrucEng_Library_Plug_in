using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CodeGenerator.Model;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.UI;
using Layer = CodeGenerator.Model.Layer;

namespace CodeGenerator
{
    public class AreaLoadViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;
        private readonly ListLoadViewModel _listLoadVm;

        private string _connectLayersLabels;

        public String ConnectLayersLabels
        {
            get => _connectLayersLabels;
            set
            {
                _connectLayersLabels = value;
                OnPropertyChanged();
            }
        }

        private String _z;

        public String Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged();
            }
        }

        private String _axes;

        public String Axes
        {
            get => _axes;
            set
            {
                _axes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Layer> _layers;

        public ObservableCollection<Layer> Layers
        {
            get => _layers;
            set
            {
                _layers = value;
                OnPropertyChanged();
                StringBuilder b = new StringBuilder();
                foreach (var layer in Layers)
                {
                    b.Append(layer.GetName() + "; ");
                }
                ConnectLayersLabels = b.ToString();
            }
        }

        public AreaLoadViewModel(ListLayerViewModel listLayerVm, ListLoadViewModel listLoadVm)
        {
            CommandConnectLayer = new RelayCommand(OnConnectLayer);
            _listLayerVm = listLayerVm;
            _listLoadVm = listLoadVm;
            StoreModelToVm();
            this.PropertyChanged += ((sender, args) => StoreVmToModel());
        }

        private void StoreVmToModel()
        {
            if (_ignoreStoreVmToModel) return;
            if (_listLoadVm.SelectedLoad.GetType() == LoadType.Area)
            {
                var l = (AreaLoad) _listLoadVm.SelectedLoad;
                l.Axes = Axes;
                l.Z = Z;
                l.Layers = Layers.ToList();
            }
        }

        private bool _ignoreStoreVmToModel = false;

        private void StoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            if (_listLoadVm.SelectedLoad.GetType() == LoadType.Area)
            {
                var l = (AreaLoad) _listLoadVm.SelectedLoad;
                Z = l.Z;
                Axes = l.Axes;
                Layers = new ObservableCollection<Layer>(l.Layers);
            }

            _ignoreStoreVmToModel = false;
        }

        public RelayCommand CommandConnectLayer;

        private void OnConnectLayer()
        {
            var dialog = new SelectLayerDialog(_listLayerVm.Layers.ToList());
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                foreach (var dialogSelectedLayer in dialog.SelectedLayers)
                {
                    Rhino.RhinoApp.WriteLine("{0}", dialogSelectedLayer.GetName());
                }

                Layers = new ObservableCollection<Layer>(dialog.SelectedLayers);
            }
        }
    }
}