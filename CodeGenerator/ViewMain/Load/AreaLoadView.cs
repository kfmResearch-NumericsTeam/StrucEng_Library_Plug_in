using System;
using System.Collections.Generic;
using System.Linq;
using CodeGenerator.Model;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace CodeGenerator
{
    /// <summary>
    /// View for area load
    /// </summary>
    public class AreaLoadView : AbstractLoadView
    {
        private readonly AreaLoadViewModel _vm;

        private TextBox _tbZ;
        private TextBox _tbAxes;

        public AreaLoadView(AreaLoadViewModel vm) : base(vm)
        {
            _vm = vm;
        }

        protected override void BindGui()
        {
            base.BindGui();
            _tbZ.Bind<String>("Text", _vm, "Z", DualBindingMode.TwoWay);
            _tbAxes.Bind<String>("Text", _vm, "Axes", DualBindingMode.TwoWay);
        }

        protected override void BuildGui()
        {
            base.BuildGui();
            _tbZ = new TextBox();
            _tbAxes = new TextBox();

            Padding = new Padding(5) { };
            Spacing = new Size(5, 5);
            AddRow(TableLayout.HorizontalScaled(new Label {Text = "z"}, _tbZ));
            AddRow(TableLayout.HorizontalScaled(new Label {Text = "Axes"}, _tbAxes));
        }
    }
}