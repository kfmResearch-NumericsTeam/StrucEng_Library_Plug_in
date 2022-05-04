using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>View for LoadConstraints</summary>
    public class LoadConstraintView : DynamicLayout
    {
        private readonly LoadConstraintViewModel _vm;


        public LoadConstraintView(LoadConstraintViewModel vm)
        {
            _vm = vm;
            BuildGui();
        }

        // XXX: Can be extension of root class
        protected IndirectBinding<string> FloatBinding(
            Expression<Func<LoadConstraintEntryViewModel, float>> f, float defVal = 0)
        {
            return Binding.Property(f).Convert(
                v => v.ToString(),
                s =>
                {
                    float i = 0;
                    return float.TryParse(s, out i) ? i : defVal;
                });
        }

        protected IndirectBinding<string> IntBinding(
            Expression<Func<LoadConstraintEntryViewModel, int>> f, int defVal = 0)
        {
            return Binding.Property(f).Convert(
                v => v.ToString(),
                s =>
                {
                    int i = 0;
                    return int.TryParse(s, out i) ? i : defVal;
                });
        }

        private void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 10);

            Add(new GroupBox
                {
                    Text = "Set Constraints",
                    Padding = new Padding(5),
                    Visible = true,
                    Content = new DynamicLayout()
                    {
                        Rows =
                        {
                            new GridView()
                            {
                                ShowHeader = true,
                                DataStore = _vm.LoadConstraints,
                                Columns =
                                {
                                    new GridColumn()
                                    {
                                        Editable = false,
                                        DataCell = new TextBoxCell()
                                        {
                                            Binding =
                                                Binding
                                                    .Property<LoadConstraintEntryViewModel,
                                                        string>(r =>
                                                        r.LayerName)
                                        },
                                        HeaderText = "Layer",
                                    },
                                    ColItem("Element Number", IntBinding(r => r.ElementNumber)),
                                    ColItem("Ex [0]", IntBinding(r => r.Ex0)),
                                    ColItem("Ex [1]", IntBinding(r => r.Ex1)),
                                    ColItem("Ex [2]", IntBinding(r => r.Ex2)),
                                    ColItem("Ey [0]", IntBinding(r => r.Ey0)),
                                    ColItem("Ey [1]", IntBinding(r => r.Ey1)),
                                    ColItem("Ey [2]", IntBinding(r => r.Ey2)),
                                    ColItem("Ez [0]", IntBinding(r => r.Ez0)),
                                    ColItem("Ez [1]", IntBinding(r => r.Ez1)),
                                    ColItem("Ez [2]", IntBinding(r => r.Ez2)),
                                }
                            },
                            new TableLayout
                            {
                                Spacing = new Size(5, 10),
                                Padding = new Padding()
                                {
                                    Top = 10,
                                    Bottom = 5,
                                },
                                Rows =
                                {
                                    new TableRow(
                                        TableLayout.AutoSized(
                                            new Button
                                            {
                                                Size = new Size(110, -1),
                                                Text = "Show Numbers...",
                                                Enabled = true,
                                                Command = _vm.ExecElementNumbers
                                            })
                                    )
                                }
                            }
                        }
                    }
                }
            );
        }

        private GridColumn ColItem(string header, IndirectBinding<string> b)
        {
            return new GridColumn()
            {
                Editable = true,
                DataCell = new TextBoxCell()
                {
                    Binding = b
                },
                HeaderText = header,
            };
        }
    }
}