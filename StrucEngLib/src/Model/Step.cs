using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StrucEngLib.Model
{
    public class Step
    {
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
            return Entries.Any(e => e.Value.Equals(o));
        }

        public bool ContainsType(StepType type)
        {
            return Entries.Any(e => e.Type.Equals(type));
        }

        public void AddEntry(StepType type, object o)
        {
            StepEntry e = new StepEntry(type, o);
            Entries.Add(e);
        }

        public ICollection<StepEntry> EntriesByType(StepType type)
        {
            return new ReadOnlyCollection<StepEntry>(Entries.Where(e => e.Type == type).ToList());
        }

        /// <summary>
        /// Get a list of all layer names included in the step
        /// </summary>
        public ICollection<string> LayerNames()
        {
            List<string> names = new List<string>();
            names.AddRange(EntriesByType(StepType.Set).Select(e => ((Set) e.Value).Name));
            names.AddRange(EntriesByType(StepType.Load)
                .SelectMany(e => ((Load) e.Value).Layers.Select(l => l.GetName())));
            return names.ToHashSet();
        }

        /// <summary> User readable summary of step </summary>
        public string Summary()
        {
            StringBuilder s = new StringBuilder();
            if (Entries == null || Entries.Count == 0)
            {
                s.Append("Step contains no entries");
            }
            else
            {
                bool multiLine = false;
                foreach (var e in Entries)
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