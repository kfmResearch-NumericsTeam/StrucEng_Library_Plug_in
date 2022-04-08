namespace CodeGenerator
{
    public class PropertyListViewModel: ViewModelBase
    {
        private readonly PropertyGroup _group;
        public PropertyGroup Group => _group;

        public PropertyListViewModel(PropertyGroup group)
        {
            _group = group;
        }
    }
}