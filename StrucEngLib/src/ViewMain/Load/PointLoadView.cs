namespace StrucEngLib
{
    /// <summary>Point Load View</summary>
    public class PointLoadView: AbstractLoadView
    {
        // XXX: This load currently has no properties
        public PointLoadView(AbstractLoadViewModel vm) : base(vm)
        {
            UiUtils.AddLabelTextRow(this, vm, "x [N]", "X", "");
            UiUtils.AddLabelTextRow(this, vm, "y [N]", "Y", "");
            UiUtils.AddLabelTextRow(this, vm, "z [N]", "Z", "");
            UiUtils.AddLabelTextRow(this, vm, "xx [Nm]", "XX", "");
            UiUtils.AddLabelTextRow(this, vm, "yy [Nm]", "YY", "");
            UiUtils.AddLabelTextRow(this, vm, "zz [Nm]", "ZZ", "");
        }
    }
}