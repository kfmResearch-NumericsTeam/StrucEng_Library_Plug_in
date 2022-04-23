using System;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class AreaLoadView : Dialog<DialogResult>
    {
        private readonly AreaLoadViewModel _vm;

        private TextBox _tbZ;
        private TextBox _tbAxes;
        private DynamicLayout _dlLoads;

        public AreaLoadView(AreaLoadViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BindGui()
        {
            _tbZ.Bind<String>("Text", _vm, "Z", DualBindingMode.TwoWay);
            _tbAxes.Bind<String>("Text", _vm, "Axes", DualBindingMode.TwoWay);
        }

        protected void BuildGui()
        {
            _tbZ = new TextBox();
            _tbAxes = new TextBox();
            Content = new DynamicLayout()
            {
                Padding = new Padding(30),
                Spacing = new Size(5, 5),

                Rows =
                {
                    new ViewSeparator() {Text = "Area Load"},
                    TableLayout.HorizontalScaled(new Label {Text = "z"}, _tbZ),
                    TableLayout.HorizontalScaled(new Label {Text = "Axes"}, _tbAxes)
                }
            };
        }
        //     AddRow(
        //     
        // _tbZ = new TextBox();
        // _tbAxes = new TextBox();
        // Rows.Add(TableLayout.HorizontalScaled(new Label {Text = "z"}, _tbZ));
        // Rows.Add(TableLayout.HorizontalScaled(new Label {Text = "Axes"}, _tbAxes));
        // AddRow(
        //     new DynamicLayout()
        //     {
        //         Padding = new Padding(30),
        //         Spacing = new Size(5, 1),
        //         Content = new Button{Text = "Connect to Layers"}
        //     }
        // );
        //
        // _dlLoads = new DynamicLayout();
        // AddRow(_dlLoads);    
        // }
    }
}