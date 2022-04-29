namespace CodeGenerator.Model
{
    public class Element : Layer
    {
        public ElementMaterialElastic ElementMaterialElastic { get; set; }

        public ElementShellSection ElementShellSection { get; set; }

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

        public string PrettyPrint()
        {
            return "Element: " + Name + ", " + ElementMaterialElastic;
        }
    }
}