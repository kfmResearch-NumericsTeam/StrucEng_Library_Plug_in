using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Eto.Forms;
using StrucEngLib.Model;

namespace StrucEngLib.Step
{
    using Layer = StrucEngLib.Model.Layer;
    using Load = StrucEngLib.Model.Load;

    /// <summary>
    /// Vm for a single entry in step view
    /// </summary>
    public class StepEntryViewModel : TreeGridItem, INotifyPropertyChanged
    {
        public Model.Step Model { set; get; }

        public string Order
        {
            get => Model.Order;
            set
            {
                Model.Order = value;
                OnPropertyChanged();
            }
        }

        public void ModelUpdated()
        {
            OnPropertyChanged(nameof(Description));
        }

        public string Description
        {
            get
            {
                if (Model == null)
                {
                    return "";
                }

                return Model.Summary();
            }
        }

        public StepEntryViewModel(LinFeMainViewModel mvm, Model.Step model)
        {
            mvm.ListLoadVm.LoadChanged += (sender, args) => { ModelUpdated(); };
            Model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}