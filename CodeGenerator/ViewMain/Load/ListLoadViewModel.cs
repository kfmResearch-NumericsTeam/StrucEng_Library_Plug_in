using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CodeGenerator.Model;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
{
    public class ListLoadViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;

        public ObservableCollection<ListItem> LoadNames { get; }

        private LoadType _loadName;

        public LoadType LoadName
        {
            get { return _loadName; }
            set
            {
                _loadName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Load> Loads { get; }

        private Load _selectedLoad;

        public Load SelectedLoad
        {
            get => _selectedLoad;
            set
            {
                _selectedLoad = value;
                OnPropertyChanged();
                UpdateContentView();
            }
        }

        private Control _loadView;

        public Control LoadView
        {
            get => _loadView;
            set
            {
                _loadView = value;
                OnPropertyChanged();
                LoadViewVisible = _loadView != null;
            }
        }

        private bool _loadViewVisible = false;
        public bool LoadViewVisible
        {
            get => _loadViewVisible;
            set
            {
                _loadViewVisible = value;
                OnPropertyChanged();
            }
        }
        
        private bool _selectLoadViewVisible = false;
        public bool SelectLoadViewVisible
        {
            get => _selectLoadViewVisible;
            set
            {
                _selectLoadViewVisible = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CommandAddLoad { get; }
        public RelayCommand CommandDeleteLoad { get; }

        public Workbench Model => _listLayerVm.Model;

        public ListLoadViewModel(ListLayerViewModel listLayerVm)
        {
            _listLayerVm = listLayerVm;
            CommandAddLoad = new RelayCommand(OnAddLoad);
            CommandDeleteLoad = new RelayCommand(OnLoadDelete);
            LoadNames = new ObservableCollection<ListItem>
            {
                new ListItem {Key = LoadType.Area.ToString(), Text = "Area"},
                // new ListItem {Key = LoadType.Gravity.ToString(), Text = "Gravity"},
            };
            Loads = new ObservableCollection<Load>(Model.Loads);
            Loads.CollectionChanged += (sender, args) =>
            {
                SelectLoadViewVisible = Loads.Count != 0;
            };
        }

        private void OnAddLoad()
        {
            Load newLoad;
            if (LoadName == LoadType.Gravity)
            {
                newLoad = new GravityLoad();
            }
            else if (LoadName == LoadType.Area)
            {
                newLoad = new AreaLoad();
            }
            else
            {
                throw new Exception("unknown load type");
            }

            Loads.Add(newLoad);
            Model.Loads.Add(newLoad);
        }

        private void OnLoadDelete()
        {
            if (SelectedLoad == null) return;
            Loads.Remove(SelectedLoad);
            Model.Loads.Add(SelectedLoad);
            SelectedLoad = null;
        }

        private void UpdateContentView()
        {
            if (SelectedLoad == null)
            {
                LoadView = null;
            }
            else if (SelectedLoad.GetType() == LoadType.Area)
            {
                var vm = new AreaLoadViewModel(_listLayerVm, this);
                var v = new AreaLoadView(vm);
                LoadView = v;
            }
            else if (SelectedLoad.GetType() == LoadType.Gravity)
            {
                LoadView = null;
            }
            else
            {
                throw new Exception("unsupported load");
            }
        }
    }
}