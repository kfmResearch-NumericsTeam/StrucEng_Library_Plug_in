using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>
    /// View to show information about displacements
    /// </summary>
    public class SetGeneralDisplacementView : DynamicLayout
    {
        private readonly LayerSetGeneralDisplacementViewModel _vm;

        public SetGeneralDisplacementView(LayerSetGeneralDisplacementViewModel vm)
        {
            _vm = vm;
            Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0};
            Spacing = new Size(5, 1);

            UiUtils.AddLabelTextRow(this, _vm, "x", "Ux", "0");
            UiUtils.AddLabelTextRow(this, _vm, "y", "Uy", "0");
            UiUtils.AddLabelTextRow(this, _vm, "z", "Uz", "0");
            UiUtils.AddLabelTextRow(this, _vm, "xx", "Rotx", "0");
            UiUtils.AddLabelTextRow(this, _vm, "yy", "Roty", "0");
            UiUtils.AddLabelTextRow(this, _vm, "zz", "Rotz", "0");
        }
    }
}