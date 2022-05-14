using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eto.Forms;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisTreeNodeItem : TreeGridItem, INotifyPropertyChanged
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

        bool? _include;

        public bool? Include
        {
            get => _include;
            set
            {
                _include = value;
                OnPropertyChanged();
            }
        }

        bool? _flagRf;

        public bool? FlagRf
        {
            get => _flagRf;
            set
            {
                _flagRf = value;
                OnPropertyChanged();
            }
        }

        bool? _flagRfx;

        public bool? FlagRfx
        {
            get => _flagRfx;
            set
            {
                _flagRfx = value;
                OnPropertyChanged();
            }
        }

        bool? _flagRfy;

        public bool? FlagRfy
        {
            get => _flagRfy;
            set
            {
                _flagRfy = value;
                OnPropertyChanged();
            }
        }

        bool? _flagRfz;

        public bool? FlagRfz
        {
            get => _flagRfz;
            set
            {
                _flagRfz = value;
                OnPropertyChanged();
            }
        }

        bool? _flagRfm;

        public bool? FlagRfm
        {
            get => _flagRfm;
            set
            {
                _flagRfm = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}