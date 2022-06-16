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


                StringBuilder s = new StringBuilder();

                if (Model.Entries == null || Model.Entries.Count == 0)
                {
                    s.Append("Step contains no entries");
                }
                else
                {
                    bool multiLine = false;
                    foreach (var e in Model.Entries)
                    {
                        if (e.Value == null)
                        {
                            continue;
                        }

                        if (e.Type == StepType.Load)
                        {
                            if (multiLine)
                            {
                                s.Append("\n");
                            }

                            var load = e.Value as Load;
                            s.Append("Load: " + load.Description + " ");
                            multiLine = true;
                        }
                        else if (e.Type == StepType.Set)
                        {
                            if (multiLine)
                            {
                                s.Append("\n");
                            }

                            var set = e.Value as Set;
                            s.Append("Set: " + set.Name + " ");
                            multiLine = true;
                        }
                    }
                }

                return s.ToString();
            }
        }

        public StepEntryViewModel(Model.Step model)
        {
            Model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}