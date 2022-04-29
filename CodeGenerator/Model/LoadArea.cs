using System.Collections.Generic;

namespace CodeGenerator.Model
{
    public class LoadArea : Load
    {
        public string Z { get; set; } = "0.03";
        public string Axes { get; set; } = "local";
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public LoadType GetType() => LoadType.Area;
    }
}