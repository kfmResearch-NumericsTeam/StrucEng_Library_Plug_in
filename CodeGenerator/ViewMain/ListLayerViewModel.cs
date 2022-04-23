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
    public class ListLayerViewModel : ViewModelBase
    {
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
        private bool _propertiesVisible = false;
        private Control _propertyContent;
        private Dictionary<Layer, LayerContext> _layerContext = new Dictionary<Layer, LayerContext>();

        public ListLayerViewModel()
        {
            Model = new Workbench();
            _layers = new ObservableCollection<Layer>(Model.Layers);

            CommandOnInspectCode = new RelayCommand(OnInspectCode, CanExecuteOnInspectCode);
            CommandOnMouseSelect = new RelayCommand(OnMouseSelect);
            CommandOnAddLayer = new RelayCommand(OnAddLayer, CanExecuteOnAddLayer);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);
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

        private bool CanExecuteOnAddLayer() => !string.IsNullOrEmpty(LayerToAdd);
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

            CommandOnInspectCode.UpdateCanExecute();
            OnPropertyChanged(nameof(Layers));
        }

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

        /*
         * MVVC Getter/Setters
         */

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
                    // PropertyContent = GetPropertyContentForLayer(_selectedLayer);
                }
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
                CommandOnAddLayer.UpdateCanExecute();
            }
        }

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

        public Control PropertyContent
        {
            get => _propertyContent;
            set
            {
                _propertyContent = value;
                OnPropertyChanged();
            }
        }

        private Control GetPropertyContentForLayer(Layer l)
        {
            if (_layerContext.ContainsKey(l))
            {
                return _layerContext[l].View;
            }

            LayerContext d = new LayerContext();
            var control = d.GetPropertyContentForLayer(l);
            _layerContext[l] = d;
            return control;
        }

        /**
         * Maps Model to Property List and stores UI, and ViewModels of Property lists
         */
        private class LayerContext
        {
            private SectionViewModel _elementSectionVm;
            private SectionViewModel _elementMaterialVm;
            private SectionViewModel _setDisplacementVm;

            public Control View;

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
                View = layout;

                return layout;
            }

            private Control _GetPropertyContentForSet(Set s)
            {
                var layout = new TableLayout()
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5),
                };
                _setDisplacementVm = new SectionViewModel(Builder.BuildDisplacement());
                layout.Rows.Add(new SectionView(_setDisplacementVm));
                View = layout;
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

            public void StoreLayerData(Layer l)
            {
                if (l.GetType() == LayerType.ELEMENT)
                {
                    Builder.MapGroupToMaterial(_elementMaterialVm.SelectedPropertyGroup, (Element) l);
                    Builder.MapGroupToSection(_elementSectionVm.SelectedPropertyGroup, (Element) l);
                }
                else if (l.GetType() == LayerType.SET)
                {
                    Builder.MapGroupToDisplacement(_setDisplacementVm.SelectedPropertyGroup, (Set) l);
                }
            }
        }
    }
}