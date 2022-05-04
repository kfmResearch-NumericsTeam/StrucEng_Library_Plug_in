using System.Collections.Generic;

namespace StrucEngLib.Model
{
    public class LoadArea : Load
    {
        public string Z { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Axes { get; set; }
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public LoadType LoadType => LoadType.Area;
    }
}