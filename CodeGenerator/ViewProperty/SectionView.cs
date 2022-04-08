using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class SectionView : DynamicLayout
    {
        private readonly SectionViewModel _vm;

        private DropDown _dbSectionSelection;
        private DynamicLayout _propertyLayout;

        public SectionView(SectionViewModel vm)
        {
            this._vm = vm;
            BuildGui();
        }

        protected void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 5);


            var sectionLayout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            Add(sectionLayout);

            sectionLayout.Rows.Add(new ViewSeparator() {Text = _vm.SectionLabel});
            sectionLayout.Rows.Add(TableLayout.HorizontalScaled((_dbSectionSelection = new DropDown { })));

            _dbSectionSelection.ItemTextBinding = Binding.Property((PropertyGroup t) => t.Label);
            _dbSectionSelection.DataStore = _vm.Groupings;
            _dbSectionSelection.Bind<PropertyGroup>("SelectedValue", _vm, "SelectedPropertyGroup",
                DualBindingMode.TwoWay);

            _propertyLayout = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            _propertyLayout.Bind<Control>("Content", _vm, "PropertyView",
                DualBindingMode.TwoWay);
            
            sectionLayout.Rows.Add(_propertyLayout);
        }
    }
}