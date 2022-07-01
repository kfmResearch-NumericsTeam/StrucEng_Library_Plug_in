using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Views;
using Size = Eto.Drawing.Size;

namespace StrucEngLib.Sm
{
    /// <summary>Renders a single analysis item for Sandwich Model</summary>
    public class SmAnalysisItemView : DynamicLayout
    {
        public SmAnalysisItemView()
        {
            GroupBox gbDetail;
            Padding = new Padding(5);
            Spacing = new Size(5, 10);

            // UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Mindestbewehrung", nameof(_vm.Mindestbewehrung), true);
            // UiUtils.AddLabelDropdownRowBoolean(layout, _vm, "Druckzoneniteration", nameof(_vm.Druckzoneniteration),
            //     true);
            // TODO Remaining fields
            
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
                        new TableRow(TextBox("Schubnachweis", "", model => model.Schubnachweis, "sia")),
                        new TableRow(TextBox("Code", "", model => model.Code, "sia")),
                        new TableRow(TextBox("axes_scale", "", model => model.AxesScale, "100")),
                        new TableRow(CheckBox("as", "xi_top", model => model.AsXiTop)),
                        new TableRow(CheckBox("as", "xi_bot", model => model.AsXiBot)),
                        new TableRow(CheckBox("as", "eta_top", model => model.AsEtaTop)),
                        new TableRow(CheckBox("as", "eta_bot", model => model.AsEtaBot)),
                        new TableRow(CheckBox("as", "z", model => model.AsZ)),
                        new TableRow(CheckBox("CC", "top", model => model.CCTop)),
                        new TableRow(CheckBox("CC", "bot", model => model.CCBot)),
                        new TableRow(CheckBox("k", "top", model => model.KTop)),
                        new TableRow(CheckBox("k", "bot", model => model.KBot)),
                        new TableRow(CheckBox("t", "top", model => model.TTop)),
                        new TableRow(CheckBox("t", "bot", model => model.TBot)),
                        new TableRow(CheckBox("psi", "top", model => model.PsiTop)),
                        new TableRow(CheckBox("psi", "bot", model => model.PsiBot)),
                        new TableRow(CheckBox("Fall", "top", model => model.FallTop)),
                        new TableRow(CheckBox("Fall", "bot", model => model.FallBot)),
                        new TableRow(CheckBox("m", "cc_top", model => model.MCcTop)),
                        new TableRow(CheckBox("m", "cc_bot", model => model.MCcBot)),
                        new TableRow(CheckBox("m", "shear_c", model => model.MShearC)),
                        new TableRow(CheckBox("m", "c_total", model => model.MCTotal)),
                    }
                }
            });

            gbDetail.BindDataContext(lab => lab.Text,
                Binding.Property<SmAnalysisItemViewModel, string>((m => m.StepName))
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

        protected static TableCell TableCellWithControl(string text, string textSubscriptSuffix,
            Control control, string toolTip = null)
        {
            var c = new TableCell()
            {
                Control = control
            };

            var l = new SubscriptLabel()
            {
                Text = text,
                TextSubscript = textSubscriptSuffix
            };
            if (control is CheckBox cb)
            {
                l.MouseDown += (sender, args) => { cb.Checked = !cb.Checked; };
            }
            else
            {
                l.MouseDown += (sender, args) => { control.Focus(); };
            }


            var c1 = new TableCell()
            {
                Control = l
            };
            var c2 = new TableCell()
            {
                Control = control
            };
            var table = TableLayout.HorizontalScaled(c1, c2);
            return table;
        }

        protected static TableCell CheckBox(string text, string textSubscriptSuffix,
            Expression<Func<SmAnalysisItemViewModel, bool?>> propertyExpression,
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
            cb.BindDataContext(p => p.Checked, Binding.Property(propertyExpression));
            return TableCellWithControl(text, textSubscriptSuffix, cb, toolTip);
        }

        // protected static TableCell DropDown(string text, string textSubscriptSuffix,
        //     Expression<Func<SmAnalysisItemViewModel, string>> propertyExpression)
        // {
        //     DropDown db = new DropDown()
        //     {
        //
        //     };
        //     // db.BindDataContext<SmAnalysisItemViewModel>(p => p.SelectedValue, Binding.Property(propertyExpression));
        //     return TableCellWithControl(text, textSubscriptSuffix, cb, toolTip);
        // }

        protected static TableCell TextBox(
            string text,
            string textSubscriptSuffix,
            Expression<Func<SmAnalysisItemViewModel, string>> propertyExpression,
            string defaultValue = "",
            string toolTip = null)
        {
            var cb = UiUtils.TextInputWithDataContextBinding(Binding.Property(propertyExpression),
                defaultValue: defaultValue);
            cb.ToolTip = toolTip;
            return TableCellWithControl(text, textSubscriptSuffix, cb, toolTip);
        }
    }
}