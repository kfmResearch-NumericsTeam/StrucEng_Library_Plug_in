using System;
using System.Collections.Generic;
using System.Text;


namespace StrucEngLib.Model
{
    public class Workbench
    {
        public List<Layer> Layers { get; set; }
        public List<Load> Loads { get; set; } = new List<Load>();
        public List<Step> Steps { get; set; } = new List<Step>();

        public Workbench()
        {
            Layers = new List<Layer>();
        }

        public Element AddElement(string name)
        {
            if (name == null) return null;
            if (name == "") return null;

            Element e = new Element() {Name = name};
            Layers.Add(e);
            return e;
        }

        public Set AddSet(string name)
        {
            if (name == null) return null;
            if (name == "") return null;

            Set e = new Set() {Name = name};
            Layers.Add(e);
            return e;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("Workbench: ");
            foreach (var l in Layers)
            {
                b.Append(l.PrettyPrint());
                b.Append(", ");
            }

            return b.ToString();
        }

        /*
         * Given A workbench, group steps according to their order (float).
         * Result is a List of steps belonging to the same order-id, ordered by order-id
         */
        public SortedDictionary<float, List<Model.Step>> GroupSteps(Workbench model)
        {
            var steps = new SortedDictionary<float, List<Model.Step>>();
            foreach (var step in model.Steps)
            {
                try
                {
                    var order = float.Parse(step.Order);
                    if (String.IsNullOrEmpty(step.Order)) continue;
                    if (steps.ContainsKey(order))
                    {
                        steps[order].Add(step);
                    }
                    else
                    {
                        steps.Add(order, new List<Model.Step>() {step});
                    }
                }
                catch (Exception)
                {
                    // XXX: We ignore invalid step numbers
                }
            }

            return steps;
        }
    }

}