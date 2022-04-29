namespace CodeGenerator.Model
{
    public class Set : Layer
    {
        public SetDisplacement SetDisplacement { get; set; }
        
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

        public string PrettyPrint()
        {
            return "Set: " + Name + ", " + SetDisplacement;
        }
    }
}