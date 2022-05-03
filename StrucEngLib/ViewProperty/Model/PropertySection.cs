using System;
using System.Collections.Generic;

namespace StrucEngLib
{
    public class PropertySection
    {
        public PropertySection() {}
        
        public PropertySection(string key, string label)
        {
            Key = key;
            Label = label;
        } 
        
        public string Label { get; set; }
        public string Key { get; set; }

        public List<PropertyGroup> Groups { get; set; } = new List<PropertyGroup>();
        
        public PropertyGroup GetByKey(string k)
        {
            foreach (var p in Groups)
            {
                if (p.Key.Equals(k))
                {
                    return p;
                }
            }

            throw new Exception("unknown Group for key: " + k);
        }
    }
}