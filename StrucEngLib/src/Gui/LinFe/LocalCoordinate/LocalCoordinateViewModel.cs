using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;

namespace StrucEngLib.LocalCoordinate
{
    using Layer = StrucEngLib.Model.Layer;

    /// <summary>Vm for LoadConstriants</summary>
    public class LoadConstraintViewModel : ViewModelBase
    {
        private readonly LinFeMainViewModel _vm;
        
        public RelayCommand ExecElementNumbers;
        public RelayCommand ExecShowCodeElementNumbers;

        private readonly ObservableCollection<LocalCoordinateEntryViewModel> _loadConstraints;
        public ObservableCollection<LocalCoordinateEntryViewModel> LoadConstraints => _loadConstraints;

        public LoadConstraintViewModel(LinFeMainViewModel vm)
        {
            _vm = vm;
            ExecElementNumbers = new RelayCommand(OnExecElementNumbers);
            ExecShowCodeElementNumbers = new RelayCommand(OnShowCodeElementNumbers);
            
            _loadConstraints = new ObservableCollection<LocalCoordinateEntryViewModel>();
            PopulateLayers(vm);
            vm.ListLayerVm.Layers.CollectionChanged += LayersOnCollectionChanged;
        }

        private void OnExecElementNumbers()
        {
            new ExecElementNumbers(_vm).ExecuteAsync(null);
        }

        private void OnShowCodeElementNumbers()
        {
            var sourceCode = new ExecElementNumbers(_vm).BuildScript();
            var dialog = new InspectPythonDialog(sourceCode);
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                sourceCode = dialog.Source;

                if (dialog.State == InspectPythonDialog.STATE_EXEC)
                {
                    new ExecElementNumbers(_vm).RunCode(sourceCode);
                }
            }
        }

        private void PopulateLayers(LinFeMainViewModel vm)
        {
            if (vm.Workbench.Layers != null)
            {
                foreach (var layer in vm.Workbench.Layers)
                {
                    if (layer.LayerType == LayerType.ELEMENT)
                    {
                        var e = layer as Element;
                        _loadConstraints.Add(new LocalCoordinateEntryViewModel(e));
                    }
                }
            }
        }

        private void LayersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                foreach (Layer newLayer in args.NewItems)
                {
                    if (newLayer.LayerType == LayerType.ELEMENT)
                    {
                        LocalCoordinateEntryViewModel c = new LocalCoordinateEntryViewModel(newLayer as Element);
                        _loadConstraints.Add(c);
                    }
                }
            }

            if (args.OldItems != null)
            {
                foreach (Layer oldLayer in args.OldItems)
                {
                    if (oldLayer.LayerType == LayerType.ELEMENT)
                    {
                        var cVm = GetConstraintEntryByOrigin(oldLayer);
                        if (cVm == null) continue;
                        _loadConstraints.Remove(cVm);
                    }
                }
            }
        }

        private LocalCoordinateEntryViewModel GetConstraintEntryByOrigin(Layer o)
        {
            foreach (var vm in LoadConstraints)
            {
                if (vm.Origin == o)
                {
                    return vm;
                }
            }

            return null;
        }
    }
}