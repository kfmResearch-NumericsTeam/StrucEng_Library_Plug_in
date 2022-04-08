using System.Collections.Generic;
using System.Xml.Serialization;
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
        
        public MaterialElastic MaterialElastic { get; set; } 
        
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
        public Displacement Displacement { get; set; }
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

    public class Displacement
    {
        public string Ux { get; set; }
        public string Uy { get; set; }
        public string Uz { get; set; }
        public string Rotx { get; set; }
        public string Roty { get; set; }
        public string Rotz { get; set; }
    }

    public class MaterialElastic
    {
        public string E { get; set; }
        public string V { get; set; }
        public string P { get; set; }
    }

    public class ShellSection
    {
        public int Thickness { get; set; }
    }

    public class AreaLoad
    {
        public string Z { get; set; }
        public string Axes { get; set; }

        public List<Layer> Elements { get; set; }
    }

    public class GravityLoad
    {
        public List<Layer> Elements { get; set; }
    }
}