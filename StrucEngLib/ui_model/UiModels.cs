using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace CodeGenerator.ui_model
{
    public class Section
    {
        public string Label { get; set; }
        public string Id { get; set; }
        public List<TextField> Components { get; set; } = new List<TextField>();

        public static Section Create(string label, string id)
        {
            Section s = new Section();
            s.Label = label;
            s.Id = id;
            return s;
        }

        public Section SetId(string id)
        {
            Id = id;
            return this;
        }

        public Section AddComponent(TextField t)
        {
            Components.Add(t);
            return this;
        }
    }

    public class TextField
    {
        TextField()
        {
        }

        public TextField(string id, string label, string def)
        {
            Label = label;
            Id = id;
            Default = def;
            Value = null;
        }

        public string Label { get; set; }
        public string Id { get; set; }
        public object Default { get; set; }
        public object Value { get; set; }
    }

    public class Wrapper<T>
    {
        public T Type { get; set; }
    }

    public class SimplePropertyModel
    {
        public string Label { get; set; }

        public List<Section> Sections { get; set; } = new List<Section>();
        public Wrapper<Section> Selected { get; set; }

        public SimplePropertyModel SetLabel(string l)
        {
            Label = l;
            return this;
        }

        public SimplePropertyModel AddSection(Section s)
        {
            Sections.Add(s);
            return this;
        }

        public static SimplePropertyModel Create(string name)
        {
            var m = new SimplePropertyModel();
            m.Label = name;
            return m;
        }
    }
}