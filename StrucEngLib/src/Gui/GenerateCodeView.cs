using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary></summary>
    public class GenerateCodeView : GroupBox
    {
        private readonly MainViewModel _vm;
        private Button _btnInspectPython;
        private Button _btnExecPython;
        private LinkButton _btnClearData;

        private void BuidGui()
        {
            Text = "Generate Code";
            Padding = new Padding(5);

            var settings = new DynamicLayout();
            UiUtils.AddLabelTextRow(settings, _vm, "Filename", "filename", "Rahmen"); // TODO

            Content = new DynamicLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(10, 10),
                Rows =
                {
                    settings,
                    new TableLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(0, 10),
                        Rows =
                        {
                            new TableRow(
                                new TableCell(
                                    (_btnInspectPython = new Button {Text = "Inspect Model..."}),
                                    true
                                ),
                                new TableCell(
                                    (_btnExecPython = new Button {Text = "Execute Model"}),
                                    true
                                )
                            ),
                            new TableRow(
                                new TableCell(
                                    (_btnClearData = new LinkButton() {Text = "Reset Data"}),
                                    false
                                ))
                        }
                    }
                }
            };
        }

        private void BindGui()
        {
            _btnExecPython.Command = _vm.LinFeMainVm.ListLayerVm.CommandOnExecuteCode;
            _btnInspectPython.Command = _vm.LinFeMainVm.ListLayerVm.CommandOnInspectCode;
            _btnClearData.Command = _vm.LinFeMainVm.ListLayerVm.CommandClearModel;
        }

        public GenerateCodeView(MainViewModel vm)
        {
            _vm = vm;
            BuidGui();
            BindGui();
        }
    }
}