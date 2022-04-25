using System.Collections.ObjectModel;
using CodeGenerator.Step;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Input;

namespace CodeGenerator
{
    /**
     * @abertschi
     */
    public class StepView : DynamicLayout
    {
        private readonly StepViewModel _stepVm;
        private DynamicLayout _layout;

        public StepView(StepViewModel stepVm)
        {
            _stepVm = stepVm;
            Build();

            stepVm.Steps.CollectionChanged += (sender, args) =>
            {
                drawLayout();
            };
        }

        private void drawLayout()
        {
            var l = new TableLayout()
            {
                Spacing = new Size(5, 5)
            };

            Collection<TableRow> rows = new Collection<TableRow>();
            foreach (var step in _stepVm.Steps)
            {
                var tbStep = new TextBox() {Text = step.Order?.ToString()};
                tbStep.Enabled = false;
                tbStep.Bind<string>("Text", step, "Order", DualBindingMode.TwoWay);

                var tb = new TextBox()
                {
                    Text = step.Label,
                    Enabled = false,
                };
                l.Rows.Add(new TableRow()
                {
                    ScaleHeight = false, Cells =
                    {
                        new TableCell((new Label() {Text = "Step: "}), false),
                        new TableCell((tbStep), false),
                        new TableCell(tb, true)
                    }
                });
            }

            _layout.Content = l;
        }

        private void Build()
        {
            AddRow(
                new GroupBox
                {
                    Text = "Select Steps",
                    Padding = new Padding(5),
                    Content = (_layout = new DynamicLayout()
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                        }
                    })
                });
        }
    }
}