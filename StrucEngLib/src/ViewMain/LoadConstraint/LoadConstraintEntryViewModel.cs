using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// Represents a single entry in the list
    /// </summary>
    public class LoadConstraintEntryViewModel : ViewModelBase
    {
        private int _elementNumber;
        private float _ex0 = 0;
        private float _ex1 = 0;
        private float _ex2 = 0;
        private float _ey0 = 0;
        private float _ey1 = 0;
        private float _ey2 = 0;
        private float _ez0 = 0;
        private float _ez1 = 0;
        private float _ez2 = 0;
        public string LayerName { get; set; }
        public Layer Origin { get; }

        public LoadConstraintEntryViewModel(Layer origin)
        {
            Origin = origin;
            ModelToVm();
        }

        public int ElementNumber
        {
            get => _elementNumber;
            set
            {
                _elementNumber = value;
                OnPropertyChanged();
                VmToModel();
            }
        }
        

        public float Ex0
        {
            get => _ex0;
            set
            {
                _ex0 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ex1
        {
            get => _ex1;
            set
            {
                _ex1 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ex2
        {
            get => _ex2;
            set
            {
                _ex2 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ey0
        {
            get => _ey0;
            set
            {
                _ey0 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ey1
        {
            get => _ey1;
            set
            {
                _ey1 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ey2
        {
            get => _ey2;
            set
            {
                _ey2 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ez0
        {
            get => _ez0;
            set
            {
                _ez0 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ez1
        {
            get => _ez1;
            set
            {
                _ez1 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        public float Ez2
        {
            get => _ez2;
            set
            {
                _ez2 = value;
                OnPropertyChanged();
                VmToModel();
            }
        }

        protected void VmToModel()
        {
            var e = (Element) Origin;
            e.Ex0 = _ex0;
            e.Ex1 = _ex1;
            e.Ex2 = _ex2;

            e.Ez0 = _ez0;
            e.Ez1 = _ez1;
            e.Ez2 = _ez2;

            e.Ey0 = _ey0;
            e.Ey1 = _ey1;
            e.Ey2 = _ey2;
            e.ElementNumber = _elementNumber;
        }

        protected void ModelToVm()
        {
            var e = (Element) Origin;
            _ex0 = e.Ex0;
            _ex1 = e.Ex1;
            _ex2 = e.Ex2;

            _ez0 = e.Ez0;
            _ez1 = e.Ez1;
            _ez2 = e.Ez2;

            _ey0 = e.Ey0;
            _ey1 = e.Ey1;
            _ey2 = e.Ey2;
            _elementNumber = e.ElementNumber;
        }
    }
}