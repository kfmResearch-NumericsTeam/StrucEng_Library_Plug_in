namespace StrucEngLib.Model
{
    public class Element : Layer
    {
        public ElementMaterialElastic ElementMaterialElastic { get; set; }

        public ElementShellSection ElementShellSection { get; set; }

        public LayerType LayerType => LayerType.ELEMENT;

        public string Name { get; set; }

        public float Ex0 { get; set; }
        public float Ex1 { get; set; }
        public float Ex2 { get; set; }
        public float Ey0 { get; set; }
        public float Ey1 { get; set; }
        public float Ey2 { get; set; }
        public float Ez0 { get; set; }
        public float Ez1 { get; set; }
        public float Ez2 { get; set; }
        public int ElementNumber { get; set; }

        public string GetName()
        {
            return Name;
        }

        public string PrettyPrint()
        {
            return "Element: " + Name + ", " + ElementMaterialElastic;
        }

        public override string ToString()
        {
            return "Element: " + Name;
        }
    }
}