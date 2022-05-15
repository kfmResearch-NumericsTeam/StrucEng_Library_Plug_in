using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrucEngLib.Model;

namespace StrucEngLib.ViewMain.Step
{
    /// <summary> VM for a single Step in step view </summary>
    public class SingleStepViewModel : ViewModelBase
    {
        // public KeyValuePair<StepType, object> Step { get; }

        public Model.Step Model { get; }

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

        public string Label
        {
            get
            {
                string res = "";
                if (Model.StepType == StepType.Load)
                {
                    StringBuilder b = new StringBuilder();
                    var l = (Load) Model.Load;

                    b.Append("Load: ");
                    b.Append(l.LoadType.GetName());
                    if (l.Layers.Count > 0)
                    {
                        b.Append(" (");
                        l.Layers.Select(layer => layer.GetName())
                            .Aggregate((s, sx) => s + "; " + sx);
                        b.Append(")");
                    }
                    else
                    {
                        b.Append(" (No layers connected)");
                    }

                    res = b.ToString();
                }
                else if (Model.StepType == StepType.Set)
                {
                    var s = (Set) Model.Set;
                    res = "Set: " + s?.GetName();
                }

                return res;
            }
        }

        public SingleStepViewModel(Model.Step step)
        {
            Model = step;
            Order = step.Order;
        }

        public override void UpdateModel()
        {
            Model.Order = Order;
        }
    }
}