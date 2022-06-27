using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Size = Eto.Drawing.Size;

namespace StrucEngLib.Analysis
{
    /// <summary>Renders a single analysis item</summary>
    public class AnalysisItemView : DynamicLayout
    {
        private Control TextControlRow(Control text, Control value)
        {
            return TableLayout.HorizontalScaled(text, value);
        }

        public AnalysisItemView()
        {
            GroupBox gbDetail;
            Padding = new Padding(5);
            Spacing = new Size(5, 10);
            
            Add(gbDetail = new GroupBox
            {
                Padding = new Padding(5),
                Visible = true,
                Content = new TableLayout
                {
                    Padding = new Padding(10, 10),
                    Spacing = new Size(10, 10),
                    Rows =
                    {
                        BinaryRow(
                            TextCell("Reaction forces"),
                            Cell("rf", i => i.Rf)
                        ),
                        BinaryRow(
                            TextCell("Reaction moments"),
                            Cell("rm", i => i.Rm)
                        ),
                        BinaryRow(
                            TextCell("Displacements"),
                            Cell("u", i => i.U)
                        ),
                        BinaryRow(
                            TextCell("Rotations"),
                            Cell("ur", i => i.Ur)
                        ),
                        BinaryRow(
                            TextCell("Concentrated forces"),
                            Cell("cf", i => i.Cf)
                        ),
                        BinaryRow(
                            TextCell("Concentrated moments"),
                            Cell("cm", i => i.Cm)
                        ),
                    }
                }
            });
            gbDetail.BindDataContext(lab => lab.Text,
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

        protected static Control BinaryRow(TableCell c1, TableCell c2)
        {
            return TableLayout.HorizontalScaled(c1, c2);
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
                Control = (new Label() {Text = text,}),
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
                        if (c != null) c.Checked = cbRoot.Checked;
                    }
                };
            }
            catch (Exception)
            {
                // XXX: Hacky/ Quick way to set children according to parent
            }

            var res = new List<TableCell>();
            res.Add(root);
            return res.Concat(children.ToList()).ToArray();
        }
    }
}