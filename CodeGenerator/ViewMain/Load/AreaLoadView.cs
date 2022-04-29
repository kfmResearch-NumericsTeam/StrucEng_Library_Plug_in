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
    public class AreaLoadView : DynamicLayout
    {
        private readonly AreaLoadViewModel _vm;

        private TextBox _tbZ;
        private TextBox _tbAxes;
        private TextBox _tbConnectLayers;
        private Button _btConnectLayers;

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
            _tbConnectLayers.Bind<String>("Text", _vm, "ConnectLayersLabels", DualBindingMode.TwoWay);
            _btConnectLayers.Command = _vm.CommandConnectLayer;
        }

        protected void BuildGui()
        {
            _tbZ = new TextBox();
            _tbAxes = new TextBox();

            Padding = new Padding(5) { };
            Spacing = new Size(5, 5);
            AddRow(SelectLayerDialog.CreateUiElement(ref _btConnectLayers, ref _tbConnectLayers));
            AddRow(TableLayout.HorizontalScaled(new Label {Text = "z"}, _tbZ));
            AddRow(TableLayout.HorizontalScaled(new Label {Text = "Axes"}, _tbAxes));
        }
    }
}