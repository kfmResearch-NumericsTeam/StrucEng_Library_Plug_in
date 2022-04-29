using System;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    /// <summary></summary>
    public abstract class AbstractLoadView : DynamicLayout
    {
        private readonly AbstractLoadViewModel _vm;
        protected TextBox _tbConnectLayers;
        protected Button _btConnectLayers;

        public AbstractLoadView(AbstractLoadViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        protected virtual void BindGui()
        {
            _tbConnectLayers.Bind<String>("Text", _vm, "ConnectLayersLabels", DualBindingMode.TwoWay);
            _btConnectLayers.Command = _vm.CommandConnectLayer;
        }

        protected virtual void BuildGui()
        {
            Padding = new Padding(5) { };
            Spacing = new Size(5, 1);
            AddRow(SelectLayerDialog.CreateUiElement(ref _btConnectLayers, ref _tbConnectLayers));
        }
    }
}