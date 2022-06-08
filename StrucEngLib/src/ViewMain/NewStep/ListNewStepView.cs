using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Input;
using StrucEngLib.Model;
using StrucEngLib.NewStep;
using StrucEngLib.Step;

namespace StrucEngLib.NewStep
{
    public class NewStepViewModel : TreeGridItem, INotifyPropertyChanged
    {
        public Model.Step Model { set; get; }

        public string Order
        {
            get => Model.Order;
            set
            {
                Model.Order = value;
                OnPropertyChanged();
            }
        }

        public void ModelUpdated()
        {
            OnPropertyChanged(nameof(Description));
        }

        public string Description
        {
            get
            {
                if (Model == null)
                {
                    return "";
                }


                StringBuilder s = new StringBuilder();

                if (Model.Entries == null || Model.Entries.Count == 0)
                {
                    s.Append("Step contains no entries");
                }
                else
                {
                    bool multiLine = false;
                    foreach (var e in Model.Entries)
                    {
                        if (e.Value == null)
                        {
                            continue;
                        }

                        if (e.Type == StepType.Load)
                        {
                            if (multiLine)
                            {
                                s.Append("\n");
                            }

                            var load = e.Value as Load;
                            s.Append("Load: " + load.Description + " ");
                            multiLine = true;
                        }
                        else if (e.Type == StepType.Set)
                        {
                            if (multiLine)
                            {
                                s.Append("\n");
                            }

                            var set = e.Value as Set;
                            s.Append("Set: " + set.Name + " ");
                            multiLine = true;
                        }
                    }
                }

                return s.ToString();
            }
        }

        public NewStepViewModel(Model.Step model)
        {
            Model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    Binding = Binding.Property<NewStepViewModel, string>(r => r.Order)
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
                        l.BindDataContext(c => c.Text, Binding.Property((NewStepViewModel m) => m.Description));
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