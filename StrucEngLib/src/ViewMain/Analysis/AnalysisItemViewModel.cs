using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eto.Forms;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisItemViewModel : ViewModelBase
    {
        private string _stepName;

        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                OnPropertyChanged();
            }
        }

        private bool? _include = false;

        public bool? Include
        {
            get => _include;
            set
            {
                _include = value;
                OnPropertyChanged();
            }
        }

        // Generated flags ---------------------------------------------------------------

        private bool? _flag_rf = false;

        public bool? Flag_rf
        {
            get => _flag_rf;
            set
            {
                _flag_rf = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rfx = false;

        public bool? Flag_rfx
        {
            get => _flag_rfx;
            set
            {
                _flag_rfx = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rfy = false;

        public bool? Flag_rfy
        {
            get => _flag_rfy;
            set
            {
                _flag_rfy = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rfz = false;

        public bool? Flag_rfz
        {
            get => _flag_rfz;
            set
            {
                _flag_rfz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rfm = false;

        public bool? Flag_rfm
        {
            get => _flag_rfm;
            set
            {
                _flag_rfm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rm = false;

        public bool? Flag_rm
        {
            get => _flag_rm;
            set
            {
                _flag_rm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rmx = false;

        public bool? Flag_rmx
        {
            get => _flag_rmx;
            set
            {
                _flag_rmx = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rmy = false;

        public bool? Flag_rmy
        {
            get => _flag_rmy;
            set
            {
                _flag_rmy = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rmz = false;

        public bool? Flag_rmz
        {
            get => _flag_rmz;
            set
            {
                _flag_rmz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_rmm = false;

        public bool? Flag_rmm
        {
            get => _flag_rmm;
            set
            {
                _flag_rmm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_u = false;

        public bool? Flag_u
        {
            get => _flag_u;
            set
            {
                _flag_u = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_ux = false;

        public bool? Flag_ux
        {
            get => _flag_ux;
            set
            {
                _flag_ux = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_uy = false;

        public bool? Flag_uy
        {
            get => _flag_uy;
            set
            {
                _flag_uy = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_uz = false;

        public bool? Flag_uz
        {
            get => _flag_uz;
            set
            {
                _flag_uz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_um = false;

        public bool? Flag_um
        {
            get => _flag_um;
            set
            {
                _flag_um = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_ur = false;

        public bool? Flag_ur
        {
            get => _flag_ur;
            set
            {
                _flag_ur = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_urx = false;

        public bool? Flag_urx
        {
            get => _flag_urx;
            set
            {
                _flag_urx = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_ury = false;

        public bool? Flag_ury
        {
            get => _flag_ury;
            set
            {
                _flag_ury = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_urz = false;

        public bool? Flag_urz
        {
            get => _flag_urz;
            set
            {
                _flag_urz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_urm = false;

        public bool? Flag_urm
        {
            get => _flag_urm;
            set
            {
                _flag_urm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cf = false;

        public bool? Flag_cf
        {
            get => _flag_cf;
            set
            {
                _flag_cf = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cfx = false;

        public bool? Flag_cfx
        {
            get => _flag_cfx;
            set
            {
                _flag_cfx = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cfy = false;

        public bool? Flag_cfy
        {
            get => _flag_cfy;
            set
            {
                _flag_cfy = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cfz = false;

        public bool? Flag_cfz
        {
            get => _flag_cfz;
            set
            {
                _flag_cfz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cfm = false;

        public bool? Flag_cfm
        {
            get => _flag_cfm;
            set
            {
                _flag_cfm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cm = false;

        public bool? Flag_cm
        {
            get => _flag_cm;
            set
            {
                _flag_cm = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cmx = false;

        public bool? Flag_cmx
        {
            get => _flag_cmx;
            set
            {
                _flag_cmx = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cmy = false;

        public bool? Flag_cmy
        {
            get => _flag_cmy;
            set
            {
                _flag_cmy = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cmz = false;

        public bool? Flag_cmz
        {
            get => _flag_cmz;
            set
            {
                _flag_cmz = value;
                OnPropertyChanged();
            }
        }


        private bool? _flag_cmm = false;

        public bool? Flag_cmm
        {
            get => _flag_cmm;
            set
            {
                _flag_cmm = value;
                OnPropertyChanged();
            }
        }
    }
}