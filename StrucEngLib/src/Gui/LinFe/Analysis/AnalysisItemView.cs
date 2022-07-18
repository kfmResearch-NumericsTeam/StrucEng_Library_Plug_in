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
        private Control TextControlRow(Control text, Control value)
        {
            return TableLayout.HorizontalScaled(text, value);
        }

        private Font Bold()
        {
            return new Font(Sans, new Label().Font.Size, FontStyle.None);
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
                        new Label() {Text = "Nodes:", Font = Bold()},
                        BinaryRow(
                            TextCell("Reaction forces"),
                            Cell("rf, (rfx, rfy, rfz, rfm)", i => i.Rf)
                        ),
                        BinaryRow(
                            TextCell("Reaction moments"),
                            Cell("rm (rmx, rmy, rmz, rmm)", i => i.Rm)
                        ),
                        BinaryRow(
                            TextCell("Displacements"),
                            Cell("u (ux, uy, uz, um)", i => i.U)
                        ),
                        BinaryRow(
                            TextCell("Rotations"),
                            Cell("ur (urx, ury, urz, rum)", i => i.Ur)
                        ),
                        BinaryRow(
                            TextCell("Concentrated forces"),
                            Cell("cf (cfx, cfy, cfz, cfm)", i => i.Cf)
                        ),
                        BinaryRow(
                            TextCell("Concentrated moments"),
                            Cell("cm (cmx, cmy, cmz, cmm)", i => i.Cm)
                        ),
                        new Label() {Text = "Elements:", Font = Bold()},
                        BinaryRow(
                            TextCell("Spring forces"),
                            Cell("spf (spfx, spfy, spfz)", i => i.SpringForces)
                        ),
                        BinaryRow(
                            TextCell("Section forces"),
                            Cell("sf (sf1, sf2, sf3)", i => i.SectionForces)
                        ),
                        BinaryRow(
                            TextCell("Shell forces"),
                            Cell("sf (sf1, sf2, sf3, sf4, sf5)", i => i.ShellForces)
                        ),

                        BinaryRow(
                            TextCell("Section moments"),
                            Cell("sm (sm1, sm2, sm3)", i => i.SectionMoments)
                        ),

                        BinaryRow(
                            TextCell("Shell moments"),
                            Cell("sm (sm1, sm2, sm3)", i => i.ShellMoments)
                        ),

                        // XXX: for now we dont include these settings
                        /*
                        BinaryRow(
                            TextCell("Section strains"),
                            Cell("se", i => i.SectionStrains)
                        ),

                        BinaryRow(
                            TextCell("Section curvatures"),
                            Cell("sk", i => i.SectionCurvatures)
                        ),
                        BinaryRow(
                            TextCell("Shell curvatures"),
                            Cell("sk", i => i.ShellCurvatures)
                        ),
                        BinaryRow(
                            TextCell("Stress (beams)"),
                            Cell("s", i => i.StressBeams)
                        ),

                        BinaryRow(
                            TextCell("Stress (shells)"),
                            Cell("s", i => i.StressShells)
                        ),
                        BinaryRow(
                            TextCell("Stress (derived)"),
                            Cell("s", i => i.StressDerived)
                        ),

                        BinaryRow(
                            TextCell("Strain (beams)"),
                            Cell("e", i => i.StrainBeams)
                        ),
                        BinaryRow(
                            TextCell("Strain (shells)"),
                            Cell("e", i => i.StrainShells)
                        ),
                        BinaryRow(
                            TextCell("Strain (derived)"),
                            Cell("e", i => i.StrainDerived)
                        ),
                        */
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