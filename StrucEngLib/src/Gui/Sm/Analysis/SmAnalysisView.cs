using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for Analysis control in sandwich model
    /// </summary>
    public class SmAnalysisView : DynamicLayout
    {
        private readonly SmAnalysisViewModel _vm;
        private Button _btnInspectPython;
        private Button _btnExecPython;
        private LinkButton _btnClearData;

        public SmAnalysisView(SmAnalysisViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 1);

            AddRow(new GroupBox
            {
                Text = "Generate Code",
                Padding = new Padding(5),
                Content = new TableLayout
                {
                    Padding = new Padding(5),
                    Spacing = new Size(10, 10),
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
            });
        }

        private void BindGui()
        {
            _btnInspectPython.Command = _vm.CommandInspectModel;
            _btnExecPython.Command = _vm.CommandExecuteModel;
            _btnClearData.Command = _vm.CommandResetData;
        }
    }
}