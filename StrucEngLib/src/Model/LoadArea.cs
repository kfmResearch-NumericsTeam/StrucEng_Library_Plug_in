using System.Collections.Generic;
using System.Text;

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

        public string Description
        {
            get
            {
                StringBuilder res = new StringBuilder();
                if (Layers != null)
                {
                    foreach (var layer in Layers)
                    {
                        res.Append(layer.GetName() + "; ");
                    }
                }

                return "Area {" + res.ToString() + "}";
            }
        }
    }
}