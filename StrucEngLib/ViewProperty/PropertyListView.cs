using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    public class PropertyListView : TableLayout
    {
        private readonly PropertyListViewModel _vm;

        public PropertyListView(PropertyListViewModel vm)
        {
            _vm = vm;
            Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0};
            Spacing = new Size(5, 1);

            foreach (var property in _vm.Group.Properties)
            {
                var tb = new TextBox();
                tb.TextBinding.Bind(() => (string) property.Value, val =>
                {
                    vm.SectionVm.OnInputChanged();
                    property.Value = val;
                });
                Rows.Add(TableLayout.HorizontalScaled(new Label {Text = property.Label}, tb));
            }
        }
    }
}