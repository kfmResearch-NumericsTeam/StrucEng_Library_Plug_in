using System.Collections.Generic;
using System.Xml.Serialization;
using Rhino.Input.Custom;

namespace CodeGenerator.Model
{
    public class Displacement
    {
        public string Ux { get; set; }
        public string Uy { get; set; }
        public string Uz { get; set; }
        public string Rotx { get; set; }
        public string Roty { get; set; }
        public string Rotz { get; set; }

        public override string ToString()
        {
            return $"Displacement: {Ux}, {Uy}, {Uz}, {Rotx}, {Rotz}, {Rotx}";
        }
    }

    public class MaterialElastic
    {
        public string E { get; set; }
        public string V { get; set; }
        public string P { get; set; }
        
        public override string ToString()
        {
            return $"MaterialElastic: {E}, {V}, {P}";
        }
    }

    public class ShellSection
    {
        public string Thickness { get; set; }
        
        public override string ToString()
        {
            return $"ShellSection: {Thickness}";
        }
    }
}