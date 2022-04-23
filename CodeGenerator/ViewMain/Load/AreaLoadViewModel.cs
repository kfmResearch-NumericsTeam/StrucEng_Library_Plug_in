using System;
using System.Collections.Generic;
using Eto.Forms;
using Rhino.DocObjects;
using Layer = CodeGenerator.Model.Layer;

namespace CodeGenerator
{
    public class AreaLoadViewModel : ViewModelBase
    {
        private String _z;

        public String Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged();
            }
        }

        private String _area;

        public String Area
        {
            get => _area;
            set
            {
                _area = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand CommandOnSelect { get; }

        public List<Layer> Layers;

        public AreaLoadViewModel()
        {
            CommandOnSelect = new RelayCommand(OnSelect);
        }

        protected void OnSelect()
        {
            
        }
    }
}