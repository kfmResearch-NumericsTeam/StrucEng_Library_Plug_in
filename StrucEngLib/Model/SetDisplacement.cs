namespace StrucEngLib.Model
{
    public class SetDisplacement
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
}