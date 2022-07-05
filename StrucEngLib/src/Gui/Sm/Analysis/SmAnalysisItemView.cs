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

            Add(gbDetail = new GroupBox
            {
                Visible = true,
                Content = new DynamicLayout()
                {
                    Spacing = new Size(5, 1),
                    Rows =
                    {
                        new TableLayout
                        {
                            Spacing = new Size(1, 2),
                            Padding = new Padding(10, 10, 10, 0),
                            Rows =
                            {
                                new TableRow(TextBox("Schubnachweis", "", model => model.Schubnachweis, "sia")),
                                new TableRow(TextBox("Code", "", model => model.Code, "sia")),
                                new TableRow(TextBox("axes_scale", "", model => model.AxesScale, "100")),
                                new TableRow(TrueFalseDropDown("Mindestbewehrung", "", model => model.Mindestbewehrung)),
                                new TableRow(TrueFalseDropDown("Druckzoneniteration", "", model => model.Druckzoneniteration))
                            }
                        },
                        new TableLayout
                        {
                            Padding = new Padding(10, 5, 10, 10),
                            Spacing = new Size(1, 5),
                            Rows =
                            {
                                new TableRow(
                                    CheckBox("as", "xi_top", model => model.AsXiTop),
                                    CheckBox("as", "xi_bot", model => model.AsXiBot),
                                    CheckBox("as", "eta_top", model => model.AsEtaTop),
                                    CheckBox("as", "eta_bot", model => model.AsEtaBot),
                                    CheckBox("as", "z", model => model.AsZ),
                                    new Label() // XXX: Last element not enlarged
                                ),
                                new TableRow(
                                    CheckBox("CC", "top", model => model.CCTop),
                                    CheckBox("CC", "bot", model => model.CCBot)),
                                new TableRow(
                                    CheckBox("k", "top", model => model.KTop),
                                    CheckBox("k", "bot", model => model.KBot)),
                                new TableRow(
                                    CheckBox("t", "top", model => model.TTop),
                                    CheckBox("t", "bot", model => model.TBot)),

                                new TableRow(
                                    CheckBox("psi", "top", model => model.PsiTop),
                                    CheckBox("psi", "bot", model => model.PsiBot)),

                                new TableRow(
                                    CheckBox("Fall", "top", model => model.FallTop),
                                    CheckBox("Fall", "bot", model => model.FallBot)),

                                new TableRow(
                                    CheckBox("m", "cc_top", model => model.MCcTop),
                                    CheckBox("m", "cc_bot", model => model.MCcBot),
                                    CheckBox("m", "shear_c", model => model.MShearC),
                                    CheckBox("m", "c_total", model => model.MCTotal)),
                            }
                        }
                    }
                }
            });

            gbDetail.BindDataContext(lab => lab.Text,
                Binding.Property<SmAnalysisItemViewModel, string>((m => m.StepName))
                    .Convert(s => "Output for Step " + s));
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
                    ToolTip = toolTip
                }),
            };
            cb.BindDataContext(p => p.Checked, Binding.Property(propertyExpression));
            return TableCellWithControl(text, textSubscriptSuffix, cb, toolTip);
        }

        protected static TableCell TrueFalseDropDown(string text, string textSubscriptSuffix,
            Expression<Func<SmAnalysisItemViewModel, object>> propertyExpression, bool selectTrue = true)
        {
            DropDown db = new DropDown()
            {
            };
            db.DataStore = new[] {"true", "false"};
            db.SelectedIndex = selectTrue ? 0 : 1;
            
            db.SelectedValueBinding.BindDataContext(
                Binding.Property(propertyExpression));
            
            return TableCellWithControl(text, textSubscriptSuffix, db, "");
        }

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