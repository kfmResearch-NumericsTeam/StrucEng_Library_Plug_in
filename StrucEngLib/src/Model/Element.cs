namespace StrucEngLib.Model
{
    public class Element : Layer
    {
        public ElementMaterialElastic ElementMaterialElastic { get; set; }

        public ElementShellSection ElementShellSection { get; set; }
        
        public LayerType LayerType => LayerType.ELEMENT;

        public string Name { get; set; }

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