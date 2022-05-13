using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>
    /// View to show information about displacements
    /// </summary>
    public class LayerDisplacementView : DynamicLayout
    {
        private readonly LayerDisplacementViewModel _vm;

        public LayerDisplacementView(LayerDisplacementViewModel vm)
        {
            _vm = vm;

            var layout = new DynamicLayout()
            {
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
                Spacing = new Size(5, 1)
            };

            UiUtils.AddLabelTextRow(layout, _vm, "x", "Ux", "");
            UiUtils.AddLabelTextRow(layout, _vm, "y", "Uy", "");
            UiUtils.AddLabelTextRow(layout, _vm, "z", "Uz", "");
            UiUtils.AddLabelTextRow(layout, _vm, "xx", "Rotx", "");
            UiUtils.AddLabelTextRow(layout, _vm, "yy", "Roty", "");
            UiUtils.AddLabelTextRow(layout, _vm, "zz", "Rotz", "");

            Add(new SelectionView("Displacements",
                new List<string>() {"General Displacement"},
                new List<Control>() {layout}));
        }
    }
}