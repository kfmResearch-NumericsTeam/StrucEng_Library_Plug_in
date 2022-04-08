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
    public class MainViewModel : ViewModelBase
    {
        private static readonly int LAYER_TYPE_ELEMENT = 0;
        private static readonly int LAYER_TYPE_SET = 1;

        public RelayCommand CommandInspectCode { get; }
        public RelayCommand CommandMouseSelect { get; }
        public RelayCommand CommandOnAddLayer { get; }
        public RelayCommand CommandOnDeleteLayer { get; }

        public Workbench Model { get; }

        private Layer _selectedLayer;
        private ObservableCollection<Layer> _layers;

        public Layer SelectedLayer
        {
            get => _selectedLayer;
            set
            {
                StoreData(_selectedLayer);
                _selectedLayer = value;
                OnPropertyChanged();
                CommandOnDeleteLayer.UpdateCanExecute();
                PropertiesVisible = _selectedLayer != null;

                // Update UI for layer properties if new layer is selected
                if (_selectedLayer != null)
                {
                    PropertyContent = GetPropertyContentForElement(_selectedLayer);
                }
            }
        }

        public void StoreData(Layer layer)
        {
            if (layer == null)
            {
                return;
            }

            if (_layerContext.ContainsKey(layer))
            {
                var ctx = _layerContext[layer];
                ctx.StoreLayerData(layer);
            }
        }

        public ObservableCollection<CodeGenerator.Model.Layer> Layers => _layers;

        public MainViewModel()
        {
            Model = new Workbench();
            _layers = new ObservableCollection<Layer>(Model.Layers);

            CommandInspectCode = new RelayCommand(OnInspectCode, CanExecuteOnInspectCode);
            CommandMouseSelect = new RelayCommand(OnMouseSelect);
            CommandOnAddLayer = new RelayCommand(OnAddLayer, CanExecuteOnAddLayer);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);
        }

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
                Rhino.RhinoApp.WriteLine("Layer cant be empty");
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
            Rhino.RhinoApp.WriteLine("Added: {0}", l);

            CommandInspectCode.UpdateCanExecute();
            OnPropertyChanged(nameof(Layers));
        }

        private bool CanExecuteOnAddLayer()
        {
            var val = !string.IsNullOrEmpty(LayerToAdd);
            return val;
        }

        private bool CanExecuteOnDeleteLayer() => SelectedLayer != null;

        private void OnInspectCode()
        {
            StoreData(_selectedLayer);
            
            PythonCodeGenerator codeGen = new PythonCodeGenerator(Model);
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
            Rhino.RhinoApp.WriteLine(fileName);
            Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }

        private bool CanExecuteOnInspectCode()
        {
            return _layers != null && _layers.Count > 0;
        }

        private int _layerToAddType = 0;

        /* 0: Element, 1: set */
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

        private string _layerToAdd;

        public string LayerToAdd
        {
            get => _layerToAdd;
            set
            {
                if (_layerToAdd == value) return;
                _layerToAdd = value;
                OnPropertyChanged();
                CommandOnAddLayer.UpdateCanExecute();
            }
        }

        private bool _propertiesVisible = false;

        public bool PropertiesVisible
        {
            get => _propertiesVisible;
            set
            {
                if (_propertiesVisible == value) return;
                _propertiesVisible = value;
                OnPropertyChanged();
            }
        }

        private Control _propertyContent;

        public Control PropertyContent
        {
            get => _propertyContent;
            set
            {
                _propertyContent = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<Layer, LayerContext> _layerContext = new Dictionary<Layer, LayerContext>();

        private Control GetPropertyContentForElement(Layer l)
        {
            if (_layerContext.ContainsKey(l))
            {
                return _layerContext[l].Control;
            }

            LayerContext d = new LayerContext();
            var control = d.GetPropertyContentForLayer(l);
            _layerContext[l] = d;
            return control;
        }

        private class LayerContext
        {
            private SectionViewModel _elementSectionVm;
            private SectionViewModel _elementMaterialVm;

            public Control Control;

            private Control _GetPropertyContentForElement(Element e)
            {
                var layout = new TableLayout()
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5),
                };
                _elementSectionVm = new SectionViewModel(Builder.BuildSections());
                layout.Rows.Add(new SectionView(_elementSectionVm));

                _elementMaterialVm = new SectionViewModel(Builder.BuildMaterials());
                layout.Rows.Add(new SectionView(_elementMaterialVm));
                Control = layout;

                return layout;
            }

            public Control GetPropertyContentForLayer(Layer e)
            {
                if (e.GetType() == LayerType.ELEMENT)
                {
                    return _GetPropertyContentForElement((Element) e);
                }
                else
                {
                    // TODO;
                    return new TableLayout();
                }
            }

            public void StoreLayerData(Layer l)
            {
                if (l.GetType() == LayerType.ELEMENT)
                {
                    Builder.MapGroupToSection(_elementMaterialVm.SelectedPropertyGroup, (Element) l);
                }
            }
        }
    }
}