using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;

namespace CodeGenerator
{
    public class SectionViewModel : ViewModelBase
    {
        private readonly PropertySection _model;
        public string SectionLabel => _model.Label;

        private Control _propertyView;

        public Control PropertyView
        {
            get => _propertyView;
            set
            {
                _propertyView = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<PropertyGroup> Groupings { get; private set; }

        private PropertyGroup _selectedPropertyGroup;

        public PropertyGroup SelectedPropertyGroup
        {
            get => _selectedPropertyGroup;
            set
            {
                _selectedPropertyGroup = value;
                OnPropertyChanged();

                var m = new PropertyListViewModel(value, this);
                var v = new PropertyListView(m);
                PropertyView = v;
            }
        }

        public SectionViewModel(PropertySection model)
        {
            _model = model;
            Groupings = new ObservableCollection<PropertyGroup>(model.Groups);;
        }

        public event EventHandler InputChanged;
        
        public void OnInputChanged()
        {
            InputChanged?.Invoke(this, new EventArgs());
        }
    }
}