using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI.Controls;
using static Eto.Drawing.FontFamilies;
using Size = Eto.Drawing.Size;

namespace StrucEngLib.Analysis
{
    /// <summary>Renders a single analysis item</summary>
    public class AnalysisItemView : DynamicLayout
    {
        private Font Bold()
        {
            return new Font(Sans, new Label().Font.Size, FontStyle.None);
        }

        private Control NoOutputAvailable()
        {
            return new TableLayout()
            {
                Padding = new Padding(10, 10),
                Spacing = new Size(10, 10),
                Rows =
                {
                    new Label() {Text = "No output available for a step containing a set."}
                }
            };
        }

        private Control Output()
        {
            return new TableLayout()
            {
                Padding = new Padding(10, 10),
                Spacing = new Size(10, 10),
                Rows =
                {
                    new Label() {Text = "Nodes:", Font = Bold()},
                    BinaryRow(
                        TextCell("Reaction forces"),
                        Cell("rf, (rfx, rfy, rfz, rfm)", i => i.Rf),
                        enabled: false
                    ),
                    BinaryRow(
                        TextCell("Reaction moments"),
                        Cell("rm (rmx, rmy, rmz, rmm)", i => i.Rm),
                        enabled: false
                    ),
                    BinaryRow(
                        TextCell("Displacements"),
                        Cell("u (ux, uy, uz, um)", i => i.U)
                    ),
                    BinaryRow(
                        TextCell("Rotations"),
                        Cell("ur (urx, ury, urz, rum)", i => i.Ur),
                        enabled: false
                    ),
                    BinaryRow(
                        TextCell("Concentrated forces"),
                        Cell("cf (cfx, cfy, cfz, cfm)", i => i.Cf),
                        enabled: false
                    ),
                    BinaryRow(
                        TextCell("Concentrated moments"),
                        Cell("cm (cmx, cmy, cmz, cmm)", i => i.Cm),
                        enabled: false
                    ),
                    new Label() {Text = "Elements:", Font = Bold()},

                    BinaryRow(
                        TextCell("Shell forces"),
                        Cell("sf (sf1, sf2, sf3, sf4, sf5)", i => i.ShellForces)
                    ),

                    BinaryRow(
                        TextCell("Section moments"),
                        Cell("sm (sm1, sm2, sm3)", i => i.SectionMoments)
                    ),
                }
            };
        }

        public AnalysisItemView(AnalysisViewModel analysisViewModel)
        {
            GroupBox gbDetail;
            Padding = new Padding(5);
            Spacing = new Size(5, 10);

            Add(gbDetail = new GroupBox
            {
                Padding = new Padding(5),
                Visible = true,
            });

            gbDetail.BindDataContext(view => view.Content,
                Binding.Property<AnalysisItemViewModel, bool>(m => m.AnalysisSettingsAllowed())
                    .Convert<Control>(b => b ? Output() : NoOutputAvailable()));

            gbDetail.BindDataContext(lab => lab.Text,
                Binding.Property<AnalysisItemViewModel, string>((m => m.StepName))
                    .Convert(s => "Output for Step " + s));
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

        protected static Control BinaryRow(TableCell c1, TableCell c2, bool enabled = true)
        {
            ClickHelp(c1, c2);
            c1.Control.Enabled = enabled;
            c2.Control.Enabled = enabled;
            if (!enabled)
            {
                c1.Control.ToolTip = "This feature is not available";
                c2.Control.ToolTip = "This feature is not available";
            }

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

        protected static TableCell TextCell(string text, bool spacing = true)
        {
            var c = new TableCell()
            {
                Control = (new Label() {Text = (spacing ? "   " : "") + text,}),
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