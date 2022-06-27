using System;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Load
{
    /// <summary>Abstract view class with base functionality for loads</summary>
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

        protected void BindGui()
        {
            _tbConnectLayers.Bind<String>(nameof(_tbConnectLayers.Text),
                _vm,
                nameof(_vm.ConnectLayersLabels),
                DualBindingMode.TwoWay);
            _btConnectLayers.Command = _vm.CommandConnectLayer;
            _tbConnectLayers.KeyDown += (sender, args) =>
            {
                if (args.Key == Keys.Backspace || args.Key == Keys.Delete)
                {
                    _vm.Layers.Clear();
                }
            };
        }

        protected void BuildGui()
        {
            Padding = new Padding(5) { };
            Spacing = new Size(5, 1);
            AddRow(SelectLayerDialog.CreateUiElement(ref _btConnectLayers, ref _tbConnectLayers));
        }
    }
}