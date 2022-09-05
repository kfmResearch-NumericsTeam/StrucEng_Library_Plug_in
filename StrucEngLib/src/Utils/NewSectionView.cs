using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Utils
{
    /// <summary>View for NewSelectionViewModel</summary>
    public class NewSelectionView<ENUM> : DynamicLayout
    {
        private readonly string _sectionLabel;
        private readonly NewSectionViewModel<ENUM> _vm;

        private DropDown _db;
        private DynamicLayout _propertyLayout;

        public NewSelectionView(string sectionLabel, NewSectionViewModel<ENUM> vm)
        {
            _vm = vm;
            _sectionLabel = sectionLabel;

            BuildGui();

            _db.BindDataContext(c => c.DataStore, (NewSectionViewModel<ENUM> m) => m.EntryNames);
            _db.SelectedKeyBinding.BindDataContext(
                Binding.Property((NewSectionViewModel<ENUM> m) => m.EntryName).EnumToString(),
                defaultContextValue: string.Empty);

            DataContext = _vm;
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

            sectionLayout.Rows.Add(new ViewSeparator() {Text = _sectionLabel});
            sectionLayout.Rows.Add(TableLayout.HorizontalScaled((_db = new DropDown { })));
            _propertyLayout = new DynamicLayout
            {
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
                Spacing = new Size(5, 5),
                Visible = true
            };

            if (_vm.EntryView != null)
            {
                _propertyLayout.Add(_vm.EntryView);
            }

            _propertyLayout.Bind<Control>(nameof(_propertyLayout.Content), _vm, nameof(_vm.EntryView),
                DualBindingMode.TwoWay);

            sectionLayout.Rows.Add(_propertyLayout);
        }
    }
}