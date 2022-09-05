using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Layer
{
    /// <summary>
    /// View to show information about general displacements
    /// </summary>
    public class SetGeneralDisplacementView : DynamicLayout
    {
        private readonly SetGeneralDisplacementViewModel _vm;

        public SetGeneralDisplacementView(SetGeneralDisplacementViewModel vm)
        {
            _vm = vm;
            Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0};
            Spacing = new Size(5, 1);

            UiUtils.AddLabelTextRow(this, _vm, "x", nameof(_vm.Ux), "0");
            UiUtils.AddLabelTextRow(this, _vm, "y", nameof(_vm.Uy), "0");
            UiUtils.AddLabelTextRow(this, _vm, "z", nameof(_vm.Uz), "0");
            UiUtils.AddLabelTextRow(this, _vm, "xx", nameof(_vm.Rotx), "0");
            UiUtils.AddLabelTextRow(this, _vm, "yy", nameof(_vm.Roty), "0");
            UiUtils.AddLabelTextRow(this, _vm, "zz", nameof(_vm.Rotz), "0");
        }
    }
}