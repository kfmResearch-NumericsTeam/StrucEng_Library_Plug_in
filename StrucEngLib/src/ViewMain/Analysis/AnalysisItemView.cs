using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Size = Eto.Drawing.Size;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisItemView : DynamicLayout
    {
        private readonly AnalysisItemViewModel _vm;
        private readonly GroupBox _gbDetail;

        public AnalysisItemView()
        {
            Add(_gbDetail = new GroupBox
            {
                Padding = new Padding(5),
                Visible = true,
                Content = new TableLayout
                {
                    Padding = new Padding(10, 10),
                    Spacing = new Size(10, 10),
                    Rows =
                    {
                        Row(
                            TextCell("Reaction forces"),
                            Cell("rf", i => i.Rf)
                        ),
                        Row(
                            TextCell("Reaction moments"),
                            Cell("rm", i => i.Rm)
                        ),
                        Row(
                            TextCell("Displacements"),
                            Cell("u", i => i.U)
                        ),
                        Row(
                            TextCell("Rotations"),
                            Cell("ur", i => i.Ur)
                        ),
                        Row(
                            TextCell("Concentrated forces"),
                            Cell("cf", i => i.Cf)
                        ),
                        Row(
                            TextCell("Concentrated moments"),
                            Cell("cm", i => i.Cm)
                        ),
                    }
                }
            });
            _gbDetail.BindDataContext(lab => lab.Text,
                Binding.Property<AnalysisItemViewModel, string>((m => m.StepName))
                    .Convert(s => "Output for Step " + s));
        }

        protected TableRow Row(params TableCell[] cells)
        {
            var r = new TableRow();
            foreach (var c in cells)
            {
                r.Cells.Add(c);
            }

            return r;
        }

        protected static TableRow Row(TableCell label, params TableCell[] args)
        {
            var r = new TableRow();
            r.Cells.Add(label);
            foreach (var c in args)
            {
                r.Cells.Add(c);
            }

            return r;
        }

        protected static TableCell TextCell(string text)
        {
            var c = new TableCell()
            {
                Control = (new Label()
                        {
                            Text = text,
                            Width = 150
                        }
                    ),
            };
            return c;
        }

        protected static TableCell Cell(string text,
            Expression<Func<AnalysisItemViewModel, bool?>> propertyExpression,
            string toolTip = null)
        {
            CheckBox _cb;
            var c = new TableCell()
            {
                Control = (_cb = new CheckBox()
                {
                    ToolTip = toolTip,
                    Text = text,
                }),
            };
            _cb.BindDataContext(p => p.Checked,
                Binding.Property(propertyExpression));
            return c;
        }

        protected static TableCell[] CellWithRoot(TableCell root,
            params TableCell[] children)
        {
            try
            {
                CheckBox cbRoot = (CheckBox) root.Control;
                cbRoot.CheckedChanged += (sender, args) =>
                {
                    foreach (var cell in children)
                    {
                        var c = cell.Control as CheckBox;
                        c.Checked = cbRoot.Checked;
                    }
                };
            }
            catch (Exception e)
            {
                // XXX: Hacky/ Quick way to set children according to parent
            }

            var res = new List<TableCell>();
            res.Add(root);
            return res.Concat(children.ToList()).ToArray();
        }
    }
}