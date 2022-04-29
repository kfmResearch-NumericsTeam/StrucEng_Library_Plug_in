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
                    b.Append(Load.LoadType.GetName());
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