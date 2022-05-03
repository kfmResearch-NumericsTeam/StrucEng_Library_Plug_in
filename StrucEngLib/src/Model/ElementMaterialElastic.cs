namespace StrucEngLib.Model
{
    public class ElementMaterialElastic
    {
        public string E { get; set; }
        public string V { get; set; }
        public string P { get; set; }
        
        public override string ToString()
        {
            return $"MaterialElastic: {E}, {V}, {P}";
        }
    }
}