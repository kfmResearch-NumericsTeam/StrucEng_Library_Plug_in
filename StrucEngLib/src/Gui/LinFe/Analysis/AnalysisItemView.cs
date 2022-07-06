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

        protected static void ClickHelp(TableCell c1, TableCell c2)
        {
            try
            {
                c1.Control.MouseDown += (sender, args) =>
                {
                    if (c2.Control is CheckBox c) c.Checked = !c.Checked;
                };
            }
            catch (Exception)
            {
                // XXX: Ignore
            }
        }

        protected static Control BinaryRow(TableCell c1, TableCell c2)
        {
            ClickHelp(c1, c2);
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
            CheckBox cb;
            var c = new TableCell()
            {
                Control = (cb = new CheckBox()
                {
                    ToolTip = toolTip,
                    Text = text,
                }),
            };
            cb.BindDataContext(p => p.Checked,
                Binding.Property(propertyExpression));
            return c;
        }
    }
}