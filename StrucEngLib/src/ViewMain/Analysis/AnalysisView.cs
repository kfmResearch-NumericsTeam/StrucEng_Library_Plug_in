using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisView : DynamicLayout
    {
        private readonly AnalysisViewModel _vm;
        private readonly DropDown _dbAdd;
        private readonly Button _btAdd;
        private readonly GroupBox _gbSelect;
        private readonly ListBox _lbSelect;
        private readonly Button _btDelete;
        private readonly GroupBox _gbDetails;
        private readonly DynamicLayout _detailLayout;

        public AnalysisView(AnalysisViewModel vm)
        {
            _vm = vm;
            AddRow(
                new GroupBox
                {
                    Text = "Add",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new TableLayout
                            {
                                Spacing = new Size(5, 10),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((_dbAdd = new DropDown { }), true),
                                        }
                                    },
                                    new TableRow(
                                        TableLayout.AutoSized(
                                            _btAdd = new Button {Text = "Add", Enabled = true})
                                    )
                                }
                            }
                        }
                    }
                });
            AddRow(
                _gbSelect = new GroupBox
                {
                    Text = "Select",
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
                                            new TableCell((_lbSelect = new ListBox()
                                            {
                                            }), true),
                                            new TableCell(
                                                TableLayout.AutoSized((_btDelete = new Button {Text = "Delete"})))
                                        }
                                    },
                                }
                            },
                        }
                    }
                });
            AddRow(
                _gbDetails = new GroupBox
                {
                    Text = "Properties",
                    Padding = new Padding(5),
                    Visible = false,
                    Content = new DynamicLayout

                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new TableLayout
                            {
                                Padding = new Padding(5),
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((_detailLayout = new DynamicLayout()), true)
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }
}