using System.Collections.Generic;

namespace CodeGenerator
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
    }
}