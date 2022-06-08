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
        private GroupBox _gbSelectSteps;
        private GridView _grid;
        private Button _btAddStep;

        public ListNewStepView(ListNewStepViewModel listStepVm)
        {
            _listStepVm = listStepVm;
            Build();
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
            _grid.AllowMultipleSelection = false;
            _grid.CellFormatting += (sender, args) =>
            {
                args.BackgroundColor = Colors.White;
                args.ForegroundColor = Colors.Black;
                args.Font = new TextBox().Font;
            };
            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Order\t",
                Editable = false,
                Expand = true,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new CustomCell()
                {
                    CreateCell = (args =>
                    {
                        var db = new DropDown()
                        {
                            Items = {"a", "b", "c"},
                        };
                        return db;
                    })
                },
                Resizable = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Description\t",
                Editable = true,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new TextBoxCell()
                {
                    // Binding = Binding.Property<AnalysisItemViewModel, bool?>(r => r.Include)
                },
                Resizable = true,
                AutoSize = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Action\t",
                Editable = false,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new CustomCell()
                {
                    CreateCell = (args =>
                    {
                        var bt = new Button()
                        {
                            Text = "Delete"
                        };

                        return bt;
                    })
                },
                Resizable = true,
                AutoSize = true,
            });


            _stepListLayout = new TableLayout()
            {
                Spacing = new Size(5, 10),
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
                            _btAddStep = new Button {Text = "Add Step...", Enabled = true})
                    )
                }
            };

            AddRow(
                (_gbSelectSteps = new GroupBox
                {
                    Text = "Settings",
                    Padding = new Padding(5),
                    Content = _stepListLayout
                }));
        }
    }
}