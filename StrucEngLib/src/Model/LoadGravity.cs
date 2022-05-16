using System.Collections.Generic;

namespace StrucEngLib.Model
{
    public class LoadGravity : Load
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        
        public LoadType LoadType => LoadType.Gravity;
        
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
    }
}