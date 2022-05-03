namespace StrucEngLib
{
    public class PropertyListViewModel: ViewModelBase
    {
        private readonly PropertyGroup _group;
        
        public readonly SectionViewModel SectionVm;
        
        public PropertyGroup Group => _group;

        public PropertyListViewModel(PropertyGroup group, SectionViewModel sectionVm)
        {
            _group = group;
            SectionVm = sectionVm;
        }
    }
}