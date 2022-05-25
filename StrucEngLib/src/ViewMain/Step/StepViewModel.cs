using System.Linq;
using System.Text;
using StrucEngLib.Model;

namespace StrucEngLib.ViewMain.Step
{
    /// <summary>
    /// VM to assign a step id to a single entry
    /// </summary>
    public class StepViewModel : ViewModelBase
    {
        public StepEntry Model { get; }
        private string _order;

        public string Order
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged();
            }
        }

        public StepViewModel(StepEntry m, string order)
        {
            Model = m;
            Order = order;
        }

        public string Label
        {
            get
            {
                string res = "";
                if (Model.Type == StepType.Load)
                {
                    StringBuilder b = new StringBuilder();
                    var l = (Load) Model.Value;

                    b.Append("Load: ");
                    b.Append(l.LoadType.GetName());
                    if (l.Layers.Count > 0)
                    {
                        b.Append(" (");
                        b.Append(l.Layers.Select(layer => layer.GetName())
                            .Aggregate((s, sx) => s + "; " + sx).ToString());
                        b.Append(")");
                    }
                    else
                    {
                        b.Append(" (No layers connected)");
                    }

                    res = b.ToString();
                }
                else if (Model.Type == StepType.Set)
                {
                    var s = (Set) Model.Value;
                    res = "Set: " + s?.GetName();
                }

                return res;
            }
        }
    }
}