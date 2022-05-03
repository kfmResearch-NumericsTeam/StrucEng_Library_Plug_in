namespace StrucEngLib
{
    public class Property
    {
        public string Key { get; set; }
        public string Label { get; set; }

        private string _value;
        
        public string Value
        {
            get => (_value == null) ? Default ?? "" : _value;
            set
            {
                _value = value;
            }
        }
        public string Default { get; set; }
    }
}