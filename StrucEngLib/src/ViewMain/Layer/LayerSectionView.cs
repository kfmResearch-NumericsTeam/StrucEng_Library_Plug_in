using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>
    /// View for Section information
    /// </summary>
    public class LayerSectionView : DynamicLayout
    {
        private readonly LayerSectionViewModel _vm;

        public LayerSectionView(LayerSectionViewModel vm)
        {
            _vm = vm;

            var layout = new DynamicLayout()
            {
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
                Spacing = new Size(5, 1)
            };
            UiUtils.AddLabelTextRow(layout, _vm, "Thickness [mm]", "Thickness", "0");
            Add(new SelectionView("Sections",
                new List<string>() {"Shell Section"},
                new List<Control>() {layout}));
        }
    }
}