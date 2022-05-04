using System.Collections.Generic;

namespace StrucEngLib.Model
{
    /// <summary>Point Load</summary>
    public class LoadPoint: Load
    {
        
        public string Z { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        
        public string ZZ { get; set; }
        public string XX { get; set; }
        public string YY { get; set; }
        
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public LoadType LoadType => LoadType.Point;
        
    }
}