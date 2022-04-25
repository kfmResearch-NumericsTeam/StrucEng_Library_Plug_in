using System.Collections.Generic;
using System.Text;


namespace CodeGenerator.Model
{
    public class Workbench
    {
        public List<Layer> Layers;
        public List<Load> Loads { get; set; } = new List<Load>();

        public Workbench()
        {
            Layers = new List<Layer>();
        }

        public Element AddElement(string name)
        {
            // TODO: Exception Handling?
            if (name == null) return null;
            if (name == "") return null;

            Element e = new Element() {Name = name};
            Layers.Add(e);
            return e;
        }

        public Set AddSet(string name)
        {
            // TODO: Exception Handling?
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
    }
}