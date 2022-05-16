using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;
using System.Windows.Input;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// Main vm for layer information
    /// </summary>
    public class ListLayerViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;

        private static readonly int LAYER_TYPE_ELEMENT = 0;
        private static readonly int LAYER_TYPE_SET = 1;

        // Commands
        public RelayCommand CommandOnInspectCode { get; }
        public RelayCommand CommandOnExecuteCode { get; }
        public RelayCommand CommandOnMouseSelect { get; }
        public RelayCommand CommandOnAddLayer { get; }
        public RelayCommand CommandOnDeleteLayer { get; }

        // MVVC
        private Layer _selectedLayer;
        private int _layerToAddType = 0; /* 0: Element, 1: set */
        private string _layerToAdd;
        private bool _selectLayerViewVisible;

        public bool SelectLayerViewVisible
        {
            get => _selectLayerViewVisible;
            set
            {
                _selectLayerViewVisible = value;
                OnPropertyChanged();
            }
        }

        public ListLayerViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            Layers = new ObservableCollection<Layer>(_mainVm.Workbench.Layers);

            CommandOnInspectCode = new RelayCommand(OnInspectCode);
            CommandOnExecuteCode = new RelayCommand(OnExecCode);
            CommandOnMouseSelect = new RelayCommand(OnMouseSelect);
            CommandOnAddLayer = new RelayCommand(OnAddLayer /*, CanExecuteOnAddLayer */);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);

            Layers.CollectionChanged += (sender, args) => { SelectLayerViewVisible = Layers.Count != 0; };
        }

        private void OnInspectCode()
        {
            var model = _mainVm.BuildModel();
            var gen = new ExecGenerateCode(_mainVm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecShowCode(_mainVm, gen.GeneratedCode).Execute(null);
            }
        }

        private void OnExecCode()
        {
            var model = _mainVm.BuildModel();
            var gen = new ExecGenerateCode(_mainVm, model);
            gen.Execute(null);
            if (gen.Success)
            {
                new ExecExecuteCode(_mainVm, gen.GeneratedCode).Execute(null);
            }
        }

        // private bool CanExecuteOnAddLayer() => !string.IsNullOrEmpty(LayerToAdd);
        private bool CanExecuteOnDeleteLayer() => SelectedLayer != null;
        private bool CanExecuteOnInspectCode() => Layers != null && Layers.Count > 0;

        private void OnDeleteLayer()
        {
            if (SelectedLayer == null) return;
            /*
             * XXX: Observable collection sets SelectedLayer = null on Remove,
             * So it is important to first remove it from Model before we remove it from viewmodel.
             */
            Layers.Remove(SelectedLayer);

            SelectedLayer = null;
            OnPropertyChanged(nameof(Layers));
        }

        private void OnAddLayer()
        {
            if (string.IsNullOrEmpty(LayerToAdd))
            {
                _mainVm.ErrorVm.ShowMessage("Layer can't be empty");
                return;
            }

            var l = LayerToAddType == LAYER_TYPE_ELEMENT
                ? Element.CreateElement(LayerToAdd)
                : Set.CreateSet(LayerToAdd);

            LayerToAdd = "";
            Layers.Add(l);
            OnPropertyChanged(nameof(Layers));
            SelectedLayer = l;
        }

        private void OnMouseSelect()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var str = RhinoUtils.SelectLayerByMouse(doc);
            if (!string.IsNullOrEmpty(str))
            {
                LayerToAdd = str;
            }
        }

        /*
         * MVVC Getter/Setters
         */

        public Layer SelectedLayer
        {
            get => _selectedLayer;
            set
            {
                _selectedLayer = value;
                OnPropertyChanged();
                CommandOnDeleteLayer.UpdateCanExecute();

                // Select Layer in doc
                if (_selectedLayer != null)
                {
                    RhinoUtils.SelectLayerByName(RhinoDoc.ActiveDoc, _selectedLayer.GetName());
                }
            }
        }

        public ObservableCollection<Layer> Layers { get; }

        public int LayerToAddType
        {
            get => _layerToAddType;
            set
            {
                if (_layerToAddType == value) return;
                _layerToAddType = value;
                OnPropertyChanged();
            }
        }

        public string LayerToAdd
        {
            get => _layerToAdd;
            set
            {
                if (_layerToAdd == value) return;
                _layerToAdd = value;
                OnPropertyChanged();
            }
        }

        public override void UpdateModel()
        {
            _mainVm.Workbench.Layers.Clear();
            foreach (var layer in Layers)
            {
                _mainVm.Workbench.Layers.Add(layer);
            }
        }
    }
}