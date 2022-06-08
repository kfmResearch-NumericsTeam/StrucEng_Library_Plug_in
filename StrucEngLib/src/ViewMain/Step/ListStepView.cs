using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Model;

namespace StrucEngLib.Step
{
    /// <summary>Main view to assign steps to entries</summary>
    public class ListStepView : DynamicLayout
    {
        private readonly ListStepViewModel _listStepVm;
        private TableLayout _stepListLayout;
        private GridView _grid;

        public ListStepView(ListStepViewModel listStepVm)
        {
            _listStepVm = listStepVm;
            Build();
            Bind();
        }

        private void Bind()
        {
            _grid.DataStore = _listStepVm.StepItems;
            _grid.DataContext = _listStepVm;
            _grid.SelectedItemBinding.BindDataContext((ListStepViewModel m) => m.SelectedStepItem);
        }

        private void Build()
        {
            _grid = new GridView()
            {
                Border = BorderType.None
            };
            _grid.AllowMultipleSelection = false;
            _grid.CellFormatting += (sender, args) =>
            {
                args.BackgroundColor = Colors.White;
                args.ForegroundColor = Colors.Black;
                args.Font = new TextBox().Font;
            };
            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Step order\t\t",
                Editable = false,
                Expand = false,
                HeaderTextAlignment = TextAlignment.Left,

                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property<StepEntryViewModel, string>(r => r.Order)
                },
                Resizable = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Description\t",
                Editable = true,
                Expand = true,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new CustomCell()
                {
                    CreateCell = (args =>
                    {
                        Label l = new Label();
                        l.BindDataContext(c => c.Text, Binding.Property((StepEntryViewModel m) => m.Description));
                        return l;
                    }),
                },
                Resizable = true,
                AutoSize = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Action\t\t",
                Editable = false,
                HeaderTextAlignment = TextAlignment.Left,
                DataCell = new CustomCell()
                {
                    CreateCell = (args =>
                    {
                        var bt1 = new LinkButton()
                        {
                            Text = "Change"
                        };
                        bt1.Command = _listStepVm.CommandChangeStep;

                        var bt2 = new LinkButton()
                        {
                            Text = "Delete"
                        };
                        bt2.Command = _listStepVm.CommandDeleteStep;
                        var l = new TableLayout
                        {
                            Spacing = new Size(5, 5),
                            Rows =
                            {
                                new TableRow(
                                    new TableCell(bt1, false),
                                    new TableCell(bt2, false))
                                {
                                }
                            }
                        };
                        return l;
                    })
                },
                Resizable = true,
                AutoSize = true,
            });


            _stepListLayout = new TableLayout()
            {
                Spacing = new Size(5, 10),
                Padding = new Padding(5),
                Rows =
                {
                    new TableRow
                    {
                        ScaleHeight = false,
                        Cells =
                        {
                            new TableCell(_grid, true),
                        }
                    },
                    new TableRow(
                        TableLayout.AutoSized(
                            new Button
                            {
                                Text = "Add Step...",
                                Enabled = true,
                                Command = _listStepVm.CommandAddStep
                            })
                    )
                }
            };

            AddRow(
                (new GroupBox
                {
                    Text = "Settings",
                    Padding = new Padding(5),
                    Content = _stepListLayout
                }));
        }
    }
}