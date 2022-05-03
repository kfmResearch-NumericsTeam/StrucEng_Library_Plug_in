using System;
using System.Collections.Generic;

namespace StrucEngLib
{
    public class PropertyGroup
    {
        public PropertyGroup(string key, string label)
        {
            Key = key;
            Label = label;
        }

        public Property GetByKey(string k)
        {
            foreach (var p in Properties)
            {
                if (p.Key.Equals(k))
                {
                    return p;
                }
            }

            throw new Exception("unknown property for key: " + k);
        }
        
        public string Label { get; set; }
        public string Key { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}