using System;
using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Input;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>View to show Step ordering</summary>
    public class ListStepView : DynamicLayout
    {
        private readonly ListStepViewModel _listStepVm;
        private DynamicLayout _stepListLayout;
        private GroupBox _gbSelectSteps;

        public ListStepView(ListStepViewModel listStepVm)
        {
            _listStepVm = listStepVm;
            Build();
            Bind();
            DrawLayout();
        }

        private void Bind()
        {
            _listStepVm.RedrawEventHandler += (sender, args) => { DrawLayout(); };
        }

        private Control NoEntry()
        {
            return new Label() {Text = "Add Load or Set first."};
        }

        private void DrawLayout()
        {
            RhinoApp.WriteLine("Dray layout, {0}", _listStepVm.Steps.Count);
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
                    RhinoApp.WriteLine("step: , {0}", step.Label);
                    var dbStep = new DropDown()
                    {
                        DataStore = _listStepVm.StepNames,
                    };
                    dbStep.Bind<string>(
                        nameof(dbStep.SelectedValue),
                        step,
                        nameof(step.Order),
                        DualBindingMode.TwoWay);

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
                            new TableCell((dbStep), false),
                            new TableCell(tb, true)
                        }
                    });
                }
            }

            _gbSelectSteps.Content = l;
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