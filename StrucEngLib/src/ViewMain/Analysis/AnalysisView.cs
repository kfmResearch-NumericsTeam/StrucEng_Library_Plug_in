using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Eto.Drawing;
using Eto.Forms;
using Rhino;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisView : DynamicLayout
    {
        private readonly AnalysisViewModel _vm;
        private GridView _grid;
        private AnalysisItemView _detailView;


        public AnalysisView(AnalysisViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BindGui()
        {
            _grid.DataStore = _vm.AnalysisViewItems;
            _grid.DataContext = _vm;

            _grid.SelectedItemBinding.BindDataContext((AnalysisViewModel m) => m.SelectedItem);
            _detailView.Bind<AnalysisItemViewModel>(nameof(_detailView.DataContext), _vm, nameof(_vm.SelectedItem));
            _detailView.Bind<bool>(nameof(_detailView.Visible), _vm, nameof(_vm.SelectedItemVisible));
            // _grid.CellDoubleClick += _vm.CellDoubleClick;
        }

        private void BuildGui()
        {
            Spacing = new Size(10, 10);

            _grid = new GridView()
            {
                Border = BorderType.None
            };
            _grid.AllowMultipleSelection = false;
            _grid.CellFormatting += (sender, args) =>
            {
                args.BackgroundColor = Colors.White;
                args.BackgroundColor = Colors.White;
                args.ForegroundColor = Colors.Black;
                args.Font = new TextBox().Font;
            };
            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Step\t\t",

                Editable = false,
                Expand = true,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property<AnalysisItemViewModel, string>(r => r.StepName),
                },

                Resizable = true,
            });

            _grid.Columns.Add(new GridColumn()
            {
                HeaderText = "Include\t",
                Editable = false,
                HeaderTextAlignment = TextAlignment.Center,
                DataCell = new CheckBoxCell()
                {
                    Binding = Binding.Property<AnalysisItemViewModel, bool?>(r => r.Include)
                },
                Resizable = true,
                AutoSize = true,
            });
            AddRow(new GroupBox
            {
                Text = "Include Steps in Output",
                Padding = new Padding(5),
                Content = _grid
            });
            
            AddRow(_detailView = new AnalysisItemView()
            {
                Visible = false
            });
        }
    }
}