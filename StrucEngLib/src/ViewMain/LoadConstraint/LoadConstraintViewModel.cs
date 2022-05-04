using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>Vm for LoadConstriants</summary>
    public class LoadConstraintViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;
        public ICommand ExecElementNumbers;

        private readonly ObservableCollection<LoadConstraintEntryViewModel> _loadConstraints;
        public ObservableCollection<LoadConstraintEntryViewModel> LoadConstraints => _loadConstraints;

        public LoadConstraintViewModel(MainViewModel vm)
        {
            _vm = vm;
            ExecElementNumbers = new ExecElementNumbers(vm);
            _loadConstraints = new ObservableCollection<LoadConstraintEntryViewModel>();
            PopulateLayers(vm);
            vm.ListLayerVm.Layers.CollectionChanged += LayersOnCollectionChanged;
        }

        private void PopulateLayers(MainViewModel vm)
        {
            if (vm.ListLayerVm.Layers != null)
            {
                foreach (var layer in vm.ListLayerVm.Layers)
                {
                    if (layer.LayerType == LayerType.ELEMENT)
                    {
                        _loadConstraints.Add(new LoadConstraintEntryViewModel(layer));
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
                        LoadConstraintEntryViewModel c = new LoadConstraintEntryViewModel(newLayer)
                        {
                            LayerName = newLayer.GetName(),
                        };
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

        private LoadConstraintEntryViewModel GetConstraintEntryByOrigin(Layer o)
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