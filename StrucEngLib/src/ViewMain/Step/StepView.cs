using System;
using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Input;
using StrucEngLib.Step;

namespace StrucEngLib
{
    
    /// <summary>View to show Step ordering</summary>
    public class StepView : DynamicLayout
    {
        private readonly ListStepViewModel _listStepVm;
        private DynamicLayout _stepListLayout;
        private GroupBox _gbSelectSteps;

        public StepView(ListStepViewModel listStepVm)
        {
            _listStepVm = listStepVm;
            Build();
            Bind();
        }

        private void Bind()
        {
            _listStepVm.RedrawEventHandler += (sender, args) => { DrawLayout(); };
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

            if (_listStepVm.Steps.Count == 0)
            {
                l.Rows.Add(NoEntry());
            }
            else
            {
                foreach (var step in _listStepVm.Steps)
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
                    Text = "Set Ordering",
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