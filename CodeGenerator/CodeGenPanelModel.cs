using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeGenerator
{
    public class CodeGenPanelModel
    {
        public string Test
        {
            get { return Test; }
            set
            {
                Rhino.RhinoApp.WriteLine("set: {0}", value);
                Test = value;
            }
        }

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

        public void DeleteLayer(Layer l)
        {
            if (CurrentLayer == l)
            {
                CurrentLayer = null;
            }

            int i = 0;
            bool found = false;
            foreach (var layer in Layers)
            {
                if (layer == l)
                {
                    Layers.RemoveAt(i);
                    found = true;
                    break;
                }

                i++;
            }

            if (!found)
            {
                throw new Exception("Invalid layer l");
            }
        }
    }

    public class Layer
    {
        public string Name { get; set; }
        
        public string KeyVal1 { get; set; }
    }

    public class LayerProperty
    {
    }
}