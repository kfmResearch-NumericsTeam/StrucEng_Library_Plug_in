using System;
using System.Collections.ObjectModel;
using CodeGenerator.Step;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Input;

namespace CodeGenerator
{
    
    /// <summary>View to show Step ordering</summary>
    public class StepView : DynamicLayout
    {
        private readonly StepViewModel _stepVm;
        private DynamicLayout _stepListLayout;
        private GroupBox _gbSelectSteps;

        public StepView(StepViewModel stepVm)
        {
            _stepVm = stepVm;
            Build();
            Bind();
        }

        private void Bind()
        {
            _stepVm.Steps.CollectionChanged += (sender, args) => { DrawLayout(); };
        }

        private Control NoEntry()
        {
            return new Label() {Text = "No data for Steps"};
        }

        private void DrawLayout()
        {
            var l = new TableLayout()
            {
                Spacing = new Size(5, 5)
            };

            if (_stepVm.Steps.Count == 0)
            {
                l.Rows.Add(NoEntry());
            }
            else
            {
                foreach (var step in _stepVm.Steps)
                {
                    var tbStep = new TextBox() {Text = step.Order};
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
            }
            _stepListLayout.Content = l;
        }

        private void Build()
        {
            AddRow(
                (_gbSelectSteps = new GroupBox
                {
                    Text = "Select Steps",
                    Padding = new Padding(5),
                    Content = (_stepListLayout = new DynamicLayout()
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            NoEntry()
                        }
                    })
                }));
        }
    }
}