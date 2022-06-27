using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Views;

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

            // layout.Add(new Label());

            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Mindestbewehrung", nameof(_vm.Mindestbewehrung), true);
            UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Druckzoneniteration", nameof(_vm.Druckzoneniteration),
                true);
            UiUtils.AddLabelTextRow(layout, _vm, "Schubnachweis", nameof(_vm.Schubnachweis), "sia");
            UiUtils.AddLabelTextRow(layout, _vm, "Code", nameof(_vm.Code), "sia");
            UiUtils.AddLabelTextRow(layout, _vm, "axes_scale", nameof(_vm.AxesScale), "100");

            var boolLayout = new TableLayout
            {
                Padding = new Padding(0, 5),
                Spacing = new Size(0, 10)
            };

            Checkbox(boolLayout, _vm, "as", "xi_top", nameof(_vm.AsXiTop));
            Checkbox(boolLayout, _vm, "as", "xi_bot", nameof(_vm.AsXiBot));
            Checkbox(boolLayout, _vm, "as","eta_top", nameof(_vm.AsEtaTop));
            Checkbox(boolLayout, _vm, "as","eta_bot", nameof(_vm.AsEtaBot));
            Checkbox(boolLayout, _vm, "as","z", nameof(_vm.AsZ));
            Checkbox(boolLayout, _vm, "CC", "top", nameof(_vm.CCTop));
            Checkbox(boolLayout, _vm, "CC", "bot", nameof(_vm.CCBot));
            Checkbox(boolLayout, _vm, "k", "top", nameof(_vm.KTop));
            Checkbox(boolLayout, _vm, "k", "bot", nameof(_vm.KBot));
            Checkbox(boolLayout, _vm, "t", "top", nameof(_vm.TTop));
            Checkbox(boolLayout, _vm, "t", "bot", nameof(_vm.TBot));
            Checkbox(boolLayout, _vm, "psi", "top", nameof(_vm.PsiTop));
            Checkbox(boolLayout, _vm, "psi", "bot", nameof(_vm.PsiBot));
            Checkbox(boolLayout, _vm, "Fall", "top", nameof(_vm.FallTop));
            Checkbox(boolLayout, _vm, "Fall", "bot", nameof(_vm.FallBot));
            Checkbox(boolLayout, _vm, "m", "cc_top", nameof(_vm.MCcTop));
            Checkbox(boolLayout, _vm, "m", "cc_bot", nameof(_vm.MCcBot));
            Checkbox(boolLayout, _vm, "m", "shear_c", nameof(_vm.MShearC));
            Checkbox(boolLayout, _vm, "m", "c_total", nameof(_vm.MCTotal));

            layout.Add(new Label());
            layout.Add(boolLayout);
        }

        private void Checkbox(
            TableLayout layout,
            object vm,
            string label,
            string subLabel,
            string propName)
        {
            var cb = new CheckBox();

            var l = new SubscriptLabel()
            {
                Text = label,
                TextSubscript = subLabel
            };
            l.MouseDown += (sender, args) => { cb.Checked = !cb.Checked; };
            cb.Bind<bool>(nameof(cb.Checked), vm, propName, DualBindingMode.TwoWay);

            var c1 = new TableCell()
            {
                Control = l
            };
            var c2 = new TableCell()
            {
                Control = cb
            };
            var table = TableLayout.HorizontalScaled(c1, c2);
            layout.Rows.Add(table);
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