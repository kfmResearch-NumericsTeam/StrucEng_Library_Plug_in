namespace StrucEngLib.Model
{
    public class ElementShellSection
    {
        public string Thickness { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(Thickness)}: {Thickness}";
        }
    }
}