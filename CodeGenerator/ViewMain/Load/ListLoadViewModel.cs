using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.AccessControl;
using CodeGenerator.Model;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
{
    /// <summary>
    /// Main Vm for load information
    /// </summary>
    public class ListLoadViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainVm;
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
                CommandDeleteLoad.UpdateCanExecute();
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

        public Workbench Model
        {
            get => _listLayerVm.Model;
        }

        public ListLoadViewModel(MainViewModel mainVm)
        {
            _mainVm = mainVm;
            _listLayerVm = mainVm.ListLayerVm;
            CommandAddLoad = new RelayCommand(OnAddLoad);
            CommandDeleteLoad = new RelayCommand(OnLoadDelete, CanLoadDelete);
            LoadNames = new ObservableCollection<ListItem>
            {
                new ListItem {Key = LoadType.Area.ToString(), Text = "Area"},
                new ListItem {Key = LoadType.Gravity.ToString(), Text = "Gravity"},
            };
            Loads = new ObservableCollection<Load>(Model.Loads);
            Loads.CollectionChanged += (sender, args) => { SelectLoadViewVisible = Loads.Count != 0; };
        }

        private void OnAddLoad()
        {
            Load newLoad;
            if (LoadName == LoadType.Gravity)
            {
                newLoad = new LoadGravity();
            }
            else if (LoadName == LoadType.Area)
            {
                newLoad = new LoadArea();
            }
            else
            {
                throw new Exception("unknown load type");
            }

            Loads.Add(newLoad);
            Model.Loads.Add(newLoad);
            OnPropertyChanged(nameof(Loads));

            SelectedLoad = newLoad;
        }

        private void OnLoadDelete()
        {
            if (SelectedLoad == null) return;
            Model.Loads.Remove(SelectedLoad);
            Loads.Remove(SelectedLoad);
            OnPropertyChanged(nameof(Loads));
            SelectedLoad = null;
        }

        private bool CanLoadDelete() => Loads.Count != 0;

        private void UpdateContentView()
        {
            if (SelectedLoad == null)
            {
                LoadView = null;
            }
            else if (SelectedLoad.LoadType == LoadType.Area)
            {
                LoadView = new AreaLoadView(new AreaLoadViewModel(_mainVm));
            }
            else if (SelectedLoad.LoadType == LoadType.Gravity)
            {
                LoadView = new GravityLoadView(new GravityLoadViewModel(_mainVm));
            }
            else
            {
                throw new Exception("unsupported load");
            }
        }
    }
}