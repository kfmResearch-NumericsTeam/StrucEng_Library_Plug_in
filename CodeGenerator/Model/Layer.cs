namespace CodeGenerator.Model
{
    public enum LayerType
    {
        ELEMENT,
        SET
    }

    public interface Layer
    {
        LayerType GetType();
        string GetName();

        string PrettyPrint();
    }

    public class Element : Layer
    {
        public MaterialElastic MaterialElastic { get; set; }
        public ShellSection ShellSection { get; set; } 

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
            return "Element: " + Name + ", " + MaterialElastic;
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

        public string PrettyPrint()
        {
            return "Set: " + Name + ", " + Displacement;
        }
    }
}