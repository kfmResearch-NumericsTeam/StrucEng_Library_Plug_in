using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using StrucEngLib.Model;
using StrucEngLib.Utils;


namespace StrucEngLib.Layer
{
    /// <summary>
    /// Main vm for layer information
    /// </summary>
    public class ListLayerViewModel : ViewModelBase
    {
        private readonly LinFeMainViewModel _mainVm;

        private static readonly int LAYER_TYPE_ELEMENT = 0; // LAYER_TYPE_SET = 1;

        // Commands
        public RelayCommand CommandClearMouseSelect { get; }
        public RelayCommand CommandOnAddLayer { get; }
        public RelayCommand CommandOnDeleteLayer { get; }

        // MVVC
        private Model.Layer _selectedLayer;
        private int _layerToAddType = -1; /* -1: unselected, 0: Element, 1: set/constraint */
        private string _layerToAdd;
        private bool _selectLayerViewVisible;
        public ObservableCollection<Model.Layer> Layers { get; }

        public bool SelectLayerViewVisible
        {
            get => _selectLayerViewVisible;
            set
            {
                _selectLayerViewVisible = value;
                OnPropertyChanged();
            }
        }

        public ListLayerViewModel(LinFeMainViewModel mainVm)
        {
            _mainVm = mainVm;
            Layers = new ObservableCollection<Model.Layer>();
            CommandOnAddLayer = new RelayCommand(OnAddLayer);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);
            CommandClearMouseSelect = new RelayCommand(OnClearMouseSelect);

            var updateSelectLayerViewVisible = new Func<bool>(() => SelectLayerViewVisible = Layers.Count != 0);
            updateSelectLayerViewVisible();
            Layers.CollectionChanged += (sender, args) => { updateSelectLayerViewVisible(); };
            UpdateViewModel();
            InstallMouseSelectionListener();
        }

        ~ListLayerViewModel()
        {
            RemoveMouseSelectionListener();
        }

        private bool CanExecuteOnDeleteLayer() => SelectedLayer != null;

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

        private void OnClearMouseSelect()
        {
            LayerToAdd = "";
            LayerToAddType = -1;
        }

        private void OnAddLayer()
        {
            if (string.IsNullOrEmpty(LayerToAdd))
            {
                _mainVm.ErrorVm.ShowMessage("Layer can't be empty. Type or select layer first");
                return;
            }

            if (LayerToAddType == -1)
            {
                _mainVm.ErrorVm.ShowMessage("Layer type must be specified. " +
                                            "Select 'Element' or 'Constraint' before adding a new layer.");
                return;
            }

            var l = LayerToAddType == LAYER_TYPE_ELEMENT
                ? Element.CreateElement(LayerToAdd)
                : Set.CreateSet(LayerToAdd);

            LayerToAdd = "";
            Layers.Add(l);
            OnPropertyChanged(nameof(Layers));
            SelectedLayer = l;
            LayerToAddType = -1;
        }

        private void InstallMouseSelectionListener()
        {
            RhinoDoc.SelectObjects += RhinoDocOnSelectObjects;
        }

        private void RemoveMouseSelectionListener()
        {
            RhinoDoc.SelectObjects -= RhinoDocOnSelectObjects;
        }

        private void RhinoDocOnSelectObjects(object sender, RhinoObjectSelectionEventArgs args)
        {
            var names = new List<string>();
            if (args.RhinoObjects != null)
            {
                foreach (var obj in args.RhinoObjects)
                {
                    try
                    {
                        var index = obj.Attributes.LayerIndex;
                        var name = args.Document.Layers[index].Name;
                        if (name != null && name.Trim() != "")
                        {
                            names.Add(name);
                        }
                    }
                    catch (Exception)
                    {
                        // XXX: Best effort, ignore
                    }
                }
            }

            if (names.Count > 0)
            {
                LayerToAdd = names[0];
            }
        }

        public Model.Layer SelectedLayer
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

        public override void UpdateViewModel()
        {
            Layers.Clear();
            foreach (var layer in _mainVm.Workbench.Layers)
            {
                Layers.Add(layer);
            }
        }
    }
}