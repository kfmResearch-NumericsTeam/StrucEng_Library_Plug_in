namespace StrucEngLib.Load
{
    /// <summary>
    /// View for area load
    /// </summary>
    public class AreaLoadView : AbstractLoadView
    {
        private readonly AreaLoadViewModel _vm;

        public AreaLoadView(AreaLoadViewModel vm) : base(vm)
        {
            _vm = vm;
            BuildGui();
        }

        private new void BuildGui()
        {
            UiUtils.AddLabelTextRow(this, _vm, "Pressure [N/mm^2]", nameof(_vm.Z), "0.03");
            UiUtils.AddLabelTextRow(this, _vm, "Surface Shear x [N/mm^2]", nameof(_vm.X), "");
            UiUtils.AddLabelTextRow(this, _vm, "Surface Shear y [N/mm^2]", nameof(_vm.Y), "");
            UiUtils.AddLabelTextRow(this, _vm, "Axes", nameof(_vm.Axes), "local");
        }
    }
}