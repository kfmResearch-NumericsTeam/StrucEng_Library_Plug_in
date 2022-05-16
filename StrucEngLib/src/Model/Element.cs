namespace StrucEngLib.Model
{
    public class Element : Layer
    {
        public ElementMaterialElastic ElementMaterialElastic { get; set; }

        public ElementShellSection ElementShellSection { get; set; }

        public LayerType LayerType => LayerType.ELEMENT;

        public string Name { get; set; }

        public ElementLoadConstraint LoadConstraint { get; set; }

        public static Layer CreateElement(string name)
        {
            if (name == null) return null;
            if (name == "") return null;
            Element e = new Element() {Name = name};
            return e;
        }

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