using System;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Gui.LinFe.Load
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
                    _vm.OnLoadSettingsChanged();
                    _vm.Layers.Clear();
                }
            };
            _tbConnectLayers.GotFocus += (sender, args) =>
            {
                _vm.RhinoSelectConnectedLayers();
            };
        }

        protected void BuildGui()
        {
            Padding = new Padding(5) { };
            Spacing = new Size(5, 1);
            AddRow(CreateUiElement(ref _btConnectLayers, ref _tbConnectLayers));
        }
        
        private Control CreateUiElement(ref Button button, ref TextBox label)
        {
            label = new TextBox();
            label.ReadOnly = true;
            label.AutoSelectMode = AutoSelectMode.Always;
            label.PlaceholderText = "No Layers connected";
            return new TableLayout
            {
                Spacing = new Size(5, 10),
                Padding = new Padding()
                {
                    Bottom = 10,
                },
                Rows =
                {
                    new TableRow
                    {
                        ScaleHeight = false, Cells =
                        {
                            new TableCell((label), true),
                            TableLayout.AutoSized(
                                button = new Button {Text = "Connect...", Enabled = true})
                        }
                    },
                }
            };
        }
    }
}