using CodeGenerator.Step;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    /**
     * @abertschi
     */
    public class StepView : DynamicLayout
    {
        private readonly StepViewModel _stepVm;

        public StepView(StepViewModel stepVm)
        {
            _stepVm = stepVm;
            Build();
        }

        private void Build()
        {
            AddRow(
                new GroupBox
                {
                    Text = "Select Steps",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new TableLayout
                            {
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((new Label() {Text = "Step: "}), false),
                                            new TableCell((new TextBox() {Text = "0"}), false),
                                            new TableCell(new TextBox(), true)
                                        }
                                    },
                                }
                            },
                        }
                    }
                });
        }
    }
}