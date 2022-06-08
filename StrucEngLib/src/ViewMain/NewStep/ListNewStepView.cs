using System;
using System.Collections.ObjectModel;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Input;
using StrucEngLib.Step;

namespace StrucEngLib.NewStep
{
    public class NewStepViewModel : ViewModelBase
    {
        private string _order;

        public string Order
        {
            get => _order;
            set
            {
                _order = value;
                RhinoApp.WriteLine("Order: {0}", value);
                OnPropertyChanged();
            }
        }


        public string Description
        {
            get => "Description";
        }
    }

    /// <summary>Main view to assign steps to entries</summary>
    public class ListNewStepView : DynamicLayout
    {
        private readonly ListNewStepViewModel _listStepVm;
        private TableLayout _stepListLayout;
        private GridView _grid;

        public ListNewStepView(ListNewStepViewModel listStepVm)
        {
            _listStepVm = listStepVm;
            Build();
            Bind();
        }

        private void Bind()
        {
            _grid.DataStore = _listStepVm.StepItems;
            _grid.DataContext = _listStepVm;
            _grid.SelectedItemBinding.BindDataContext((ListNewStepViewModel m) => m.SelectedStepItem);
        }

        private void Build()
        {
            _grid = new GridView()
            {
                Border = BorderType.None
            };
            _grid.AllowEmptySelection = false; // XXX: Needed otherwise dropbox not clickable if outside of grid
            _grid.AllowMultipleSelection = false;
            _grid.CellFormatting += (sender, args) =>
            {
                args.BackgroundColor = Colors.White;
                args.ForegroundColor = Colors.Black;
                args.Font = new TextBox().Font;
            };
            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Order\t\t",
                Editable = true,
                Expand = false,
                HeaderTextAlignment = TextAlignment.Left,

                DataCell = new CustomCell()
                {
                    CreateCell = (args =>
                    {
                        Spacing = new Size(5, 5);
                        Padding = new Padding(5, 5, 5, 5);
                        var db = new DropDown()
                        {
                            DataStore = _listStepVm.StepNames,
                        };
                        db.Bind<string>(
                            nameof(db.SelectedValue),
                            db.DataContext,
                            nameof(NewStepViewModel.Order),
                            DualBindingMode.TwoWay);

                        return db;
                    })
                },
                Resizable = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Description\t",
                Editable = true,
                Expand = true,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property<NewStepViewModel, string>(r => r.Description)
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
                        var bt = new LinkButton()
                        {
                            Text = "Delete"
                        };
                        bt.Command = _listStepVm.CommandDeleteStep;
                        return bt;
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