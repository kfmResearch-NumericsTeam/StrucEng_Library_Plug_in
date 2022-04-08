using System.Collections.Generic;
using Rhino.Input.Custom;

namespace CodeGenerator.model
{
    public class Workbench
    {
        public List<Layer> Layers;

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
    }

    public enum LayerType
    {
        ELEMENT,
        SET
    }

    public interface Layer
    {
        LayerType GetType();
        string GetName();
    }

    public class Element : Layer
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return "Element: " + Name;
        }

        public LayerType GetType()
        {
            return LayerType.ELEMENT;
        }

        public string GetName()
        {
            return Name;
        }
    }

    public class Set : Layer
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return "Set: " + Name;
        }

        public LayerType GetType()
        {
            return LayerType.SET;
        }

        public string GetName()
        {
            return Name;
        }
    }
}