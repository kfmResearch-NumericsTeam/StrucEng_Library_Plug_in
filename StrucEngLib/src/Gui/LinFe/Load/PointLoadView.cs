using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Load
{
    /// <summary>Point Load View</summary>
    public class PointLoadView : AbstractLoadView
    {
        // XXX: This load currently has no properties
        public PointLoadView(PointLoadViewModel vm) : base(vm)
        {
            UiUtils.AddLabelTextRow(this, vm, "x [N]", nameof(vm.X), "");
            UiUtils.AddLabelTextRow(this, vm, "y [N]", nameof(vm.Y), "");
            UiUtils.AddLabelTextRow(this, vm, "z [N]", nameof(vm.Z), "");
            UiUtils.AddLabelTextRow(this, vm, "xx [Nm]", nameof(vm.XX), "");
            UiUtils.AddLabelTextRow(this, vm, "yy [Nm]", nameof(vm.YY), "");
            UiUtils.AddLabelTextRow(this, vm, "zz [Nm]", nameof(vm.ZZ), "");
        }
    }
}