using System.Collections.Generic;

namespace CodeGenerator.Model
{
    public class LoadGravity : Load
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        
        public LoadType LoadType => LoadType.Gravity;
    }
}