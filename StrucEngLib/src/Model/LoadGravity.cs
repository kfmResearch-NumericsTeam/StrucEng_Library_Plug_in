using System.Collections.Generic;
using System.Text;

namespace StrucEngLib.Model
{
    public class LoadGravity : Load
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();

        public LoadType LoadType => LoadType.Gravity;

        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }

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

                return "Gravity (" + res.ToString() + ")";
            }
        }
    }
}

