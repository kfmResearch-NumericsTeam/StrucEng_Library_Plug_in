using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for general settings of sandwich model
    /// </summary>
    public class SmSettingView : DynamicLayout
    {
        private readonly SmSettingViewModel _vm;
        private DropDown _dbSteps;
        private Label _lbNoStepsAdded;

        public SmSettingView(SmSettingViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 1);
            DynamicLayout layout;
            AddRow(
                new GroupBox
                {
                    Text = "Settings",
                    Padding = new Padding(5),
                    Content = layout = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                        }
                    }
                });

            layout.Add(TableLayout.HorizontalScaled(_lbNoStepsAdded = new Label()
                {Text = "No steps added to LinFe Model.\n", Visible = false}));

            layout.AddRow(TableLayout.HorizontalScaled(new Label {Text = "Step"}, (_dbSteps = new DropDown())));
            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Mindestbewehrung", nameof(_vm.Mindestbewehrung), true);
            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Druckzoneniteration", nameof(_vm.Druckzoneniteration),
                true);
            UiUtils.AddLabelTextRow(layout, _vm, "Schubnachweis", nameof(_vm.Schubnachweis), "sia");
            UiUtils.AddLabelTextRow(layout, _vm, "axes_scale", nameof(_vm.AxesScale), "100");
        }

        private void BindGui()
        {
            DataContext = _vm;
            _dbSteps.DataContext = _vm;
            _dbSteps.DataStore = _vm.StepNames;
            _dbSteps.ItemTextBinding = Binding.Property((string order) => "Step " + order);
            _dbSteps.Bind<string>(nameof(_dbSteps.SelectedValue), _vm, nameof(_vm.SelectedStepName));
            _lbNoStepsAdded.Bind<bool>(nameof(_lbNoStepsAdded.Visible), _vm, nameof(_vm.NoStepsAdded));
        }
    }
}