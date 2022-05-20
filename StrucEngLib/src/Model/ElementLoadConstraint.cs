namespace StrucEngLib.Model
{
    /// <summary></summary>
    public class ElementLoadConstraint
    {
        public int ElementNumber { get; set; }
        public int Ex0 { get; set; }
        public int Ex1 { get; set; }
        public int Ex2 { get; set; }
        public int Ey0 { get; set; }
        public int Ey1 { get; set; }
        public int Ey2 { get; set; }
        public int Ez0 { get; set; }
        public int Ez1 { get; set; }
        public int Ez2 { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(ElementNumber)}: {ElementNumber}, {nameof(Ex0)}: {Ex0}, {nameof(Ex1)}: {Ex1}," +
                $" {nameof(Ex2)}: {Ex2}, {nameof(Ey0)}: {Ey0}, {nameof(Ey1)}: {Ey1}," +
                $" {nameof(Ey2)}: {Ey2}, {nameof(Ez0)}: {Ez0}, {nameof(Ez1)}:" +
                $" {Ez1}, {nameof(Ez2)}: {Ez2}";
        }
    }
}