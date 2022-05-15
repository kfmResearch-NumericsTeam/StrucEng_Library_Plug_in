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
                            CellWithRoot(Cell("rf", i => i.Flag_rf),
                                Cell("rfx", i => i.Flag_rfx),
                                Cell("rfy", i => i.Flag_rfy),
                                Cell("rfz", i => i.Flag_rfz),
                                Cell("rfm", i => i.Flag_rfm))
                        ),
                        Row(
                            TextCell("Reaction moments"),
                            CellWithRoot(Cell("rm", i => i.Flag_rm),
                                Cell("rmx", i => i.Flag_rmx),
                                Cell("rmy", i => i.Flag_rmy),
                                Cell("rmz", i => i.Flag_rmz),
                                Cell("rmm", i => i.Flag_rmm))
                        ),
                        Row(
                            TextCell("Displacements"),
                            CellWithRoot(Cell("u", i => i.Flag_u),
                                Cell("ux", i => i.Flag_ux),
                                Cell("uy", i => i.Flag_uy),
                                Cell("uz", i => i.Flag_uz),
                                Cell("um", i => i.Flag_um))
                        ),
                        Row(
                            TextCell("Rotations"),
                            CellWithRoot(Cell("ur", i => i.Flag_ur),
                                Cell("urx", i => i.Flag_urx),
                                Cell("ury", i => i.Flag_ury),
                                Cell("urz", i => i.Flag_urz),
                                Cell("urm", i => i.Flag_urm))
                        ),
                        Row(
                            TextCell("Concentrated forces"),
                            CellWithRoot(Cell("cf", i => i.Flag_cf),
                                Cell("cfx", i => i.Flag_cfx),
                                Cell("cfy", i => i.Flag_cfy),
                                Cell("cfz", i => i.Flag_cfz),
                                Cell("cfm", i => i.Flag_cfm))
                        ),
                        Row(
                            TextCell("Concentrated moments"),
                            CellWithRoot(Cell("cm", i => i.Flag_cm),
                                Cell("cmx", i => i.Flag_cmx),
                                Cell("cmy", i => i.Flag_cmy),
                                Cell("cmz", i => i.Flag_cmz),
                                Cell("cmm", i => i.Flag_cmm))
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
                }),
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