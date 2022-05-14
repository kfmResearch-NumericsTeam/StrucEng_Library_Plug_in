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

        public AnalysisView(AnalysisViewModel vm)
        {
            _vm = vm;

            TreeGridView view = new TreeGridView()
            {
                // Height = 500
                BackgroundColor = Colors.White,
            };
            
            
            view.CellFormatting += (sender, e) =>
            {
                e.Font = new TextBox().Font;
                e.BackgroundColor = Colors.Transparent;
                e.ForegroundColor = Colors.Transparent;
                e.Column.Style
                
            };
            
            view.BackgroundColor = Colors.Transparent;

            

            view.Columns.Add(new GridColumn()
            {
                HeaderText = "Step",
                Editable = false,
                DataCell = new TextBoxCell()
                {
                },
                AutoSize = true, Resizable = true,
            });
            view.Columns.Add(new GridColumn()
            {
                HeaderText = "Include",
                Editable = true,
                DataCell = new CheckBoxCell()
                {
                },
                AutoSize = false, Resizable = true,
            });
            view.Columns.Add(new GridColumn()
            {
                HeaderText = "Flags",
                Editable = true,
                DataCell = new AnalysisTreeNodeCell()
                {
                },
                AutoSize = true, Resizable = true,
            });

            var c = new TreeGridItemCollection();
            c.Add(new AnalysisTreeNodeItem());
            view.DataStore = c;


            // AddRow(UiUtils.GenerateTitle("Step 5: Define Analysis Steps"));
            AddRow(
                new GroupBox
                {
                    Text = "",
                    Padding = new Padding(5),
                    Content = new TableLayout
                    {
                        Spacing = new Size(10, 10),
                        Rows =
                        {
                            view
                        }
                    }
                });
        }
    }
}