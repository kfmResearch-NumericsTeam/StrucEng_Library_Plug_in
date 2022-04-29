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

        public List<Layer2> Layers { get; set; } = new List<Layer2>();

        public Layer2 CurrentLayer2 { get; set; }

        public void AddNewLayer(string name, LayerType type)
        {
            Layer2 l = new Layer2();
            l.Name = name;
            CurrentLayer2 = l;
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

        public void DeleteLayer(Layer2 l)
        {
            if (CurrentLayer2 == l)
            {
                CurrentLayer2 = null;
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

    public class Layer2
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
        public Wrapper<Section> Section { get; set; }
        public Wrapper<Section> Material { get; set; }
    }

    public class SetProperty
    {
        public Wrapper<Section> Displacement;
    }
}