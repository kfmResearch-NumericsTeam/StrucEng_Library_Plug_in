using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisTreeNodeCell : CustomCell
    {
        public AnalysisTreeNodeCell()
        {
        }

        protected TableRow Row(params TableCell[] cells)
        {
            var r = new TableRow(cells)
            {
            };
            return r;
        }

        protected TableCell Cell(string text, Expression<Func<AnalysisTreeNodeItem, bool?>> propertyExpression,
            string toolTip = "")
        {
            CheckBox _cb;
            var c = new TableCell()
            {
                Control = (_cb = new CheckBox()
                {
                    ToolTip = toolTip,
                    Text = text,
                }),
                ScaleWidth = false,
            };
            _cb.BindDataContext(p => p.Checked, Binding.Property(propertyExpression));
            return c;
        }

        protected override Control OnCreateCell(CellEventArgs args)
        {
            var RF = "Reaction forces";
            var l = new TableLayout
            {
                Spacing = new Size(10, 50),
                Rows =
                {
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                    Row(
                        Cell("rf", i => i.FlagRf, RF),
                        Cell("rfx", i => i.FlagRf, RF),
                        Cell("rfy", i => i.FlagRf, RF),
                        Cell("rfz", i => i.FlagRf, RF),
                        Cell("rfm", i => i.FlagRf, RF)
                    ),
                    
                }
            };
            return l;
        }
    }
}