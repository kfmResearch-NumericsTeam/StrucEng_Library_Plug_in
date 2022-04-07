using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CodeGenerator.ui_model;

namespace CodeGenerator
{
    public class CodeGenPanelModel
    {
        public string LayerToAdd { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public Layer CurrentLayer { get; set; }

        public void AddNewLayer(string name, LayerType type)
        {
            Layer l = new Layer();
            l.Name = name;
            CurrentLayer = l;
            l.Type = type;
            if (type == LayerType.ELEMENT)
            {
                l.ElementProperty = new ElementProperty();
            }
            else if (type == LayerType.SET)
            {
                l.SetProperty = new SetProperty();
            }
            else
            {
                throw new SystemException("Unknown layer type");
            }
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

        public LayerType Type { get; set; }

        public ElementProperty ElementProperty { get; set; }

        public SetProperty SetProperty { get; set; }

        public string Thickness { get; set; }
    }

    public enum LayerType
    {
        ELEMENT,
        SET
    }

    public class ElementProperty
    {
        public Section Section { get; set; }
        public Section Material { get; set; }
    }

    public class SetProperty
    {
        public Section Displacement;
    }
}