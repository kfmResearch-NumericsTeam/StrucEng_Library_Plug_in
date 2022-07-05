using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Eto.Drawing;
using Eto.Forms;
using Rhino;

namespace StrucEngLib.Sm
{
    /// <summary>View for sandwich analysis</summary>
    public class SmAnalysisView : DynamicLayout
    {
        private readonly SmAnalysisViewModel _vm;
        private GridView _grid;
        private SmAnalysisItemView _detailView;

        public SmAnalysisView(SmAnalysisViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BindGui()
        {
            DataContext = _vm;
            _grid.DataStore = _vm.AnalysisViewItems;
            _grid.DataContext = _vm;

            _grid.SelectedItemBinding.BindDataContext((SmAnalysisViewModel m) => m.SelectedItem);
            _detailView.Bind<SmAnalysisItemViewModel>(nameof(_detailView.DataContext), _vm, nameof(_vm.SelectedItem));
            _detailView.Bind<bool>(nameof(_detailView.Visible), _vm, nameof(_vm.SelectedItemVisible));
            _detailView.BindDataContext(v => v.DataContext, (SmAnalysisViewModel m) => m.SelectedItem);
        }

        private void BuildGui()
        {
            Spacing = new Size(10, 10);
            Padding = new Padding(5);

            _grid = new GridView()
            {
                Border = BorderType.None
            };
            _grid.AllowMultipleSelection = false;
            _grid.CellFormatting += (sender, args) =>
            {
                args.BackgroundColor = Colors.White;
                args.ForegroundColor = Colors.Black;
                args.Font = new TextBox().Font;
            };
            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Step\t\t\t\t\t\t\t",
                Editable = false,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property<SmAnalysisItemViewModel, string>(r => r.StepName)
                        .Convert(step => "Step " + step + "")
                },

                Resizable = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Include\t\t",
                Editable = true,
                HeaderTextAlignment = TextAlignment.Left,
                DataCell = new CheckBoxCell()
                {
                    Binding = Binding.Property<SmAnalysisItemViewModel, bool?>(r => r.Include)
                },
                Resizable = true,
                AutoSize = true,
            });
            AddRow(new GroupBox
            {
                Text = "Include Steps in Output",
                Padding = new Padding(5),
                
                Content = new DynamicLayout(_grid)
                {
                    Spacing = new Size(5, 10),
                    Padding = new Padding(5),
                }
            });
            
            AddRow(_detailView = new SmAnalysisItemView()
            {
                Spacing = new Size(5, 10),
                Padding = new Padding(5),
                Visible = true
            });
        }
    }
}