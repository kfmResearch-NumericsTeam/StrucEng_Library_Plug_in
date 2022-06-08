using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrucEngLib.Model
{
    public class Step
    {
        public static string OrderExcluded = "Excluded";

        public string Order { get; set; }

        public List<StepEntry> Entries = new List<StepEntry>();

        public AnalysisSetting Setting { get; set; }

        public void RemoveStepEntryWithValue(object o)
        {
            var entry = Entries.FirstOrDefault(e => e.Value == o);
            Entries.Remove(entry);
        }

        public bool Contains(object o)
        {
            return Entries.Select(e => e.Value).Contains(o);
        }

        public void AddEntry(StepType type, object o)
        {
            StepEntry e = new StepEntry(type, o);
            Entries.Add(e);
        }
    }

    public class StepEntry
    {
        public StepEntry()
        {
        }

        public StepEntry(StepType type, object value)
        {
            Type = type;
            Value = value;
        }

        public StepType Type { get; set; }
        public object Value { get; set; }
    }
}