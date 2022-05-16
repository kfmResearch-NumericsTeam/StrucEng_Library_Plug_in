namespace StrucEngLib
{
    /// <summary>Gravity Load View</summary>
    public class GravityLoadView: AbstractLoadView
    {
        // XXX: This load currently has no properties
        public GravityLoadView(AbstractLoadViewModel vm) : base(vm)
        {
            UiUtils.AddLabelTextRow(this, vm, "x", "X", "0.0");
            UiUtils.AddLabelTextRow(this, vm, "y", "Y", "0.0");
            UiUtils.AddLabelTextRow(this, vm, "z", "Z", "1.0");
        }
    }
}