using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;
using CodeGenerator.model;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class MainViewModel : ViewModelBase
    {
        private static readonly int LAYER_TYPE_ELEMENT = 0;
        private static readonly int LAYER_TYPE_SET = 1;
        public RelayCommand CommandInspectCode { get; }
        public RelayCommand CommandGenerateModel { get; }
        public RelayCommand CommandMouseSelect { get; }
        public RelayCommand CommandOnAddLayer { get; }
        public RelayCommand CommandOnDeleteLayer { get; }

        public Workbench Model { get; }

        private CodeGenerator.model.Layer _selectedLayer;
        private ObservableCollection<CodeGenerator.model.Layer> _layers;

        public CodeGenerator.model.Layer SelectedLayer
        {
            get => _selectedLayer;
            set
            {
                _selectedLayer = value;
                OnPropertyChanged();
                CommandOnDeleteLayer.UpdateCanExecute();
                PropertiesVisible = _selectedLayer != null;
            }
        }

        public ObservableCollection<CodeGenerator.model.Layer> Layers => _layers;

        public MainViewModel()
        {
            Model = new Workbench();
            _layers = new ObservableCollection<model.Layer>(Model.Layers);

            CommandInspectCode = new RelayCommand(OnInspectCode, CanExecuteOnInspectCode);
            CommandGenerateModel = new RelayCommand(OnGenerateModel, CanExecuteOnGenerateModel);
            CommandMouseSelect = new RelayCommand(OnMouseSelect);
            CommandOnAddLayer = new RelayCommand(OnAddLayer, CanExecuteOnAddLayer);
            CommandOnDeleteLayer = new RelayCommand(OnDeleteLayer, CanExecuteOnDeleteLayer);
            PropertyContent = CreatePropertyContent();
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

            model.Layer l;
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

        private void OnGenerateModel()
        {
        }

        private bool CanExecuteOnGenerateModel()
        {
            return false;
        }

        private bool CanExecuteOnInspectCode()
        {
            return false;
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

        public Control CreatePropertyContent()
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            SectionViewModel sectVm = new SectionViewModel(Builder.BuildSections());
            SectionView sectView = new SectionView(sectVm);
            layout.Rows.Add(sectView);

            SectionViewModel matVm = new SectionViewModel(Builder.BuildMaterials());
            SectionView matView = new SectionView(matVm);
            layout.Rows.Add(matView);

            return layout;
        }
    }
}