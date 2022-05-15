using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StrucEngLib.Model
{
    public class NewStep
    {
        public float Order { get; set; }
        
        public List<NewStepEntry> Entries = new List<NewStepEntry>();

        public void RemoveStepEntryWithValue(object o)
        {
            var entry = Entries.FirstOrDefault(e => e.Value == o);
            Entries.Remove(entry);
        }

        public bool Contains(object o)
        {
            return Entries.Select(e => e.Value).Contains(o);
        }
    }

    public class NewStepEntry
    {
        public NewStepEntry(StepType type, object value)
        {
            Type = type;
            Value = value;
        }

        public StepType Type { get; set; }
        public object Value { get; set; }
    }

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