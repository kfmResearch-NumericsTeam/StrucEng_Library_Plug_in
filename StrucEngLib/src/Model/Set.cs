namespace StrucEngLib.Model
{
    /// <summary>
    /// Set model
    /// </summary>
    public class Set : Layer
    {
        public SetDisplacementType SetDisplacementType { get; set; } = SetDisplacementType.NONE;

        public SetGeneralDisplacement SetGeneralDisplacement { get; set; }

        public string Name { get; set; }
        
        public LayerType LayerType => LayerType.SET;

        public static Set CreateSet(string name)
        {
            if (name == null) return null;
            if (name == "") return null;

            Set e = new Set() {Name = name};
            return e;
        }
        public override string ToString()
        {
            return "Set: " + Name;
        }
        
        public string GetName()
        {
            return Name;
        }
    }
}