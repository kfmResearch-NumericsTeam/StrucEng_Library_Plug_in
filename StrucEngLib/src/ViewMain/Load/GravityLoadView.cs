namespace StrucEngLib
{
    /// <summary>Gravity Load View</summary>
    public class GravityLoadView : AbstractLoadView
    {
        // XXX: This load currently has no properties
        public GravityLoadView(GravityLoadViewModel vm) : base(vm)
        {
            UiUtils.AddLabelTextRow(this, vm, "x", nameof(vm.X), "0.0");
            UiUtils.AddLabelTextRow(this, vm, "y", nameof(vm.Y), "0.0");
            UiUtils.AddLabelTextRow(this, vm, "z", nameof(vm.Z), "1.0");
        }
    }
}
