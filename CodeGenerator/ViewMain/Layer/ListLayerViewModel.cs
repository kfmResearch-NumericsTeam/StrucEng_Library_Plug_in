using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;
using System.Windows.Input;
using CodeGenerator.Model;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
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
        public RelayCommand CommandOnMouseSelect { get; }
        public RelayCommand CommandOnAddLayer { get; }
        public RelayCommand CommandOnDeleteLayer { get; }

        public Workbench Model { get; }

        // MVVC
        private Layer _selectedLayer;
        private ObservableCollection<Layer> _layers;
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
            Model = new Workbench();
            
            _layers = new ObservableCollection<Layer>(Model.Layers);

            CommandOnInspectCode = new RelayCommand(OnInspectCode, CanExecuteOnInspectCode);
            CommandOnMouseSelect = new RelayCommand(OnMouseSelect);
            CommandOnAddLayer = new RelayCommand(OnAddLayer /*, CanExecuteOnAddLayer */);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);

            Layers.CollectionChanged += (sender, args) => { SelectLayerViewVisible = Layers.Count != 0; };
        }

        // private bool CanExecuteOnAddLayer() => !string.IsNullOrEmpty(LayerToAdd);
        private bool CanExecuteOnDeleteLayer() => SelectedLayer != null;
        private bool CanExecuteOnInspectCode() => _layers != null && _layers.Count > 0;

        private void OnDeleteLayer()
        {
            if (SelectedLayer == null) return;

            _layers.Remove(SelectedLayer);
            Model.Layers.Remove(SelectedLayer);
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

            Layer l;
            if (LayerToAddType == LAYER_TYPE_ELEMENT)
            {
                l = Model.AddElement(LayerToAdd);
            }
            else
            {
                l = Model.AddSet(LayerToAdd);
            }

            LayerToAdd = "";
            _layers.Add(l);

            CommandOnInspectCode.UpdateCanExecute();
            OnPropertyChanged(nameof(Layers));
        }

        private void OnInspectCode()
        {
            PythonCodeGenerator codeGen = new PythonCodeGenerator(Model);
            var valMsgs = codeGen.ValidateModel();
            if (valMsgs.Count != 0)
            {
                _mainVm.ErrorVm.ShowMessages(valMsgs);
                return;
            }

            var sourceCode = codeGen.Generate();

            var dialog = new InspectPythonDialog(sourceCode);
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                sourceCode = dialog.Source;

                if (dialog.State == InspectPythonDialog.STATE_EXEC)
                {
                    OnGenerateModel(sourceCode);
                }
            }
        }

        private void OnMouseSelect()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var str = RhinoUtils.SelectLayer(doc);
            if (!string.IsNullOrEmpty(str))
            {
                LayerToAdd = str;
            }
        }

        protected void OnGenerateModel(string source)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, source);
            Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
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
            }
        }

        public ObservableCollection<CodeGenerator.Model.Layer> Layers => _layers;

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
                //CommandOnAddLayer.UpdateCanExecute();
            }
        }
    }
}