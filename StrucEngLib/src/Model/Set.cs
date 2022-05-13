namespace StrucEngLib.Model
{
    public class Set : Layer
    {
        public SetDisplacementType SetDisplacementType { get; set; } = SetDisplacementType.NONE;

        public SetGeneralDisplacement SetGeneralDisplacement { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return "Set: " + Name;
        }

        public LayerType LayerType => LayerType.SET;

        public string GetName()
        {
            return Name;
        }

        public string PrettyPrint()
        {
            return "Set: " + Name + ", " + SetGeneralDisplacement;
        }
    }
}