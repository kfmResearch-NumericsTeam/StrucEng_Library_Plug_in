using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrucEngLib.Model;

namespace StrucEngLib.ViewMain.Step
{
    /// <summary> VM for a single Step in step view </summary>
    public class SingleStepViewModel : ViewModelBase
    {
        public KeyValuePair<StepType, object> Step { get; }

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
                if (Step.Key == StepType.Load)
                {
                    StringBuilder b = new StringBuilder();
                    var l = (Load) Step.Value;

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
                else if (Step.Key == StepType.Set)
                {
                    var s = (Set) Step.Value;
                    res = "Set: " + s?.GetName();
                }

                return res;
            }
        }

        public SingleStepViewModel(KeyValuePair<StepType, object> step, string order = "")
        {
            Step = step;
            Order = order;
        }
    }
}