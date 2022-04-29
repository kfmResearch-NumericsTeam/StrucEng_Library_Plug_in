using System;
using System.Text;

namespace CodeGenerator.Model
{
    public enum StepType
    {
        Load,
        Set
    };


    public class Step
    {
        public string Order { get; set; } = null; // null is uninitialized

        public StepType StepType { get; set; }

        public Load Load { get; set; }
        public Set Set { get; set; }

        public object getOrderObject()
        {
            if (StepType == StepType.Load) return Load;
            if (StepType == StepType.Set) return Set;
            return null;
        }

        public string getSummary()
        {
            string res = "";
            if (StepType == StepType.Load)
            {
                StringBuilder b = new StringBuilder();
                if (Load != null)
                {
                    b.Append(Load.GetType().GetName());
                    b.Append(" (");
                    foreach (var l in Load.Layers)
                    {
                        b.Append(l.GetName() + ";");
                    }

                    b.Append(")");
                    res = b.ToString();
                }
            }
            else
            {
                res = Set?.GetName();
            }

            return res;
        }

        public Step(StepType type)
        {
            StepType = type;
        }
    }
}