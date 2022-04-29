using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CodeGenerator.Model;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
{
    /// <summary>Abstract vm for load related views</summary>
    public abstract class AbstractLoadViewModel : ViewModelBase
    {
        protected readonly ListLayerViewModel ListLayerVm;
        protected readonly ListLoadViewModel ListLoadVm;

        protected string _connectLayersLabels;
        public string ConnectLayersLabels
        {
            get => _connectLayersLabels;
            set
            {
                _connectLayersLabels = value;
                OnPropertyChanged();
            }
        }
        
        protected ObservableCollection<Layer> _layers;
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

        public AbstractLoadViewModel(MainViewModel mainVm)
        {
            CommandConnectLayer = new RelayCommand(OnConnectLayer);
            ListLayerVm = mainVm.ListLayerVm;
            ListLoadVm = mainVm.ListLoadVm;
            this.PropertyChanged += ((sender, args) => DoStoreVmToModel());
            DoStoreModelToVm();
        }

        /// <summary>
        /// Overwrite this method to store vm data to model 
        /// </summary>
        protected virtual void StoreVmToModel() {}
        
        /// <summary>
        /// Overwrite this method to store model data to vm
        /// </summary>
        protected virtual void StoreModelToVm() {}
        
        private void DoStoreVmToModel()
        {
            if (_ignoreStoreVmToModel) return;
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Area)
            {
                var l = ListLoadVm.SelectedLoad;
                l.Layers = Layers.ToList();
                StoreVmToModel();
            }
        }

        private bool _ignoreStoreVmToModel = false;
        private void DoStoreModelToVm()
        {
            _ignoreStoreVmToModel = true;
            StoreModelToVm();
            var l = ListLoadVm.SelectedLoad;
            Layers = new ObservableCollection<Layer>(l.Layers);
            _ignoreStoreVmToModel = false;
        }

        public RelayCommand CommandConnectLayer;

        protected void OnConnectLayer()
        {
            var dialog = new SelectLayerDialog(ListLayerVm.Layers.ToList());
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                Layers = new ObservableCollection<Layer>(dialog.SelectedLayers);
            }
        }
    }
}