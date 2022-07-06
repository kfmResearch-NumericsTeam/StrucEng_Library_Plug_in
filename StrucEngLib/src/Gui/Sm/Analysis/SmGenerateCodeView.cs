using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for Analysis control in sandwich model
    /// </summary>
    public class SmGenerateCodeView : GroupBox
    {
        private readonly SmGenerateCodeViewModel _vm;
        private Button _btnInspectPython;
        private Button _btnExecPython;
        private LinkButton _btnClearData;

        public SmGenerateCodeView(SmGenerateCodeViewModel vm)
        {
            _vm = vm;
            BuidGui();
            BindGui();
        }

        private void BuidGui()
        {
            Text = "Generate Code for Sandwich Model";
            Padding = new Padding(5, 0);

            var settings = new DynamicLayout()
            {
                Padding = new Padding(5, 0, 5, 0),
                Spacing = new Size(10, 0),
            };
            UiUtils.AddLabelTextRow(settings, _vm, "Name", nameof(SmGenerateCodeViewModel.FileName), "C:\\Temp\\Rahmen");

            Content = new DynamicLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(10, 5),
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
            _btnExecPython.Command = _vm.CommandExecuteModel;
            _btnInspectPython.Command = _vm.CommandInspectModel;
            _btnClearData.Command = _vm.CommandResetData;
        }
    }
}