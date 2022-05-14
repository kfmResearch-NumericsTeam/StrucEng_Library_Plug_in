using System;
using System.Text;

namespace StrucEngLib.Model
{
    public enum StepType
    {
        Load,
        Set
    };


    public class Step
    {
        public string Order { get; set; }
        public StepType StepType { get; set; }
        public Load Load { get; set; }
        public Set Set { get; set; }

        public string GetSummary()
        {
            string res = "";
            if (StepType == StepType.Load)
            {
                StringBuilder b = new StringBuilder();
                if (Load != null)
                {
                    b.Append("Load: ");
                    b.Append(Load.LoadType.GetName());
                    if (Load.Layers.Count > 0)
                    {
                        b.Append(" (");
                        foreach (var l in Load.Layers)
                        {
                            b.Append(l.GetName() + ";");
                        }

                        b.Append(")");
                    }
                    else
                    {
                        b.Append(" (No layers connected)");
                    }

                    res = b.ToString();
                }
            }
            else
            {
                res = "Set: " + Set?.GetName();
            }

            return res;
        }

        public Step(StepType type)
        {
            StepType = type;
        }
    }
}