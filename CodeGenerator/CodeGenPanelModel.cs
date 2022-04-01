using System.Collections;
using System.Collections.Generic;

namespace CodeGenerator
{
    public class CodeGenPanelModel
    {
        public string LayerToAdd { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();
        
        public Layer CurrentLayer { get; set; }

        public void AddNewLayer(string name)
        {
            Layer l = new Layer();
            l.Name = name;
            CurrentLayer = l;
            Layers.Add(l);
        }
    }

    public class Layer
    {
        public string Name { get; set; }
        public List<LayerProperty> Properties { get; set; }
    }

    public class LayerProperty
    {
        
    }
}