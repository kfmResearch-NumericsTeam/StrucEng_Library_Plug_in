using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeGenerator.Model;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
{
    public class Load
    {
    }

    public class LoadType
    {
        public int Type { get; }
        public String Name { get; }

        public LoadType(int type, String name)
        {
            Type = type;
            Name = name;
        }
    }

    public class GravityLoad
    {
        public List<Layer> Layers { get; set; }
    }

    public class AreaLoad
    {
        public String Z = "0.03";
        public String Axes = "local";
        public List<Layer> Layers { get; set; }
    }

    public class AddLoadViewModel : ViewModelBase
    {
        public static LoadType AREA = new LoadType(0, "Area");
        public static LoadType GRAVITY = new LoadType(1, "Gravity");
        
        public ObservableCollection<LoadType> LoadsToAdd = new ObservableCollection<LoadType>();

        private Control _loadViews;

        public Control LoadViews
        {
            get => _loadViews;
            set
            {
                _loadViews = value;
                OnPropertyChanged();
            }
        }

        private LoadType _loadToAdd;
        public LoadType SelectedLoadToAdd
        {
            get => _loadToAdd;
            set
            {
                _loadToAdd = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CommandAddLoad { get; }

        public AddLoadViewModel()
        {
            LoadsToAdd.Add(AREA);
            LoadsToAdd.Add(GRAVITY);
            CommandAddLoad = new RelayCommand(OnAddLoad);
        }

        private void OnAddLoad()
        {
            Rhino.RhinoApp.WriteLine("AddLoad");
            if (SelectedLoadToAdd == null)
            {
                return;
            }
            Rhino.RhinoApp.WriteLine("{0}", SelectedLoadToAdd);

            if (SelectedLoadToAdd == AREA)
            {
                var vm = new AreaLoadViewModel();
                var dialog = new AreaLoadView(vm);
                var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
                if (dialogRc == Eto.Forms.DialogResult.Ok)
                {
                
                }
            }
        }
    }
}