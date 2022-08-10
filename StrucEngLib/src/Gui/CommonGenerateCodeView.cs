using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Gui;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// Abstract view with control buttons to generate code
    /// </summary>
    public abstract class CommonGenerateCodeView : GroupBox
    {
        private readonly CommonGenerateCodeViewModel _vm;
        private readonly string _titleText;
        protected Button _btnInspectPython;
        protected Button _btnExecPython;
        protected LinkButton _btnClearData;
        private CheckBox _cbExperimentalExec;

        protected CommonGenerateCodeView(CommonGenerateCodeViewModel vm, string titleText)
        {
            _vm = vm;
            _titleText = titleText;
            BuidGui();
            BindGui();
        }

        private void BuidGui()
        {
            Text = _titleText;
            Padding = new Padding(5);

            var settings = new DynamicLayout()
            {
                Padding = new Padding(5, 0),
                Spacing = new Size(10, 0),
            };
            UiUtils.AddLabelTextRow(settings, _vm, "Name",
                nameof(CommonGenerateCodeViewModel.FileName),
                "C:\\Temp\\Rahmen");

            Content = new DynamicLayout()
            {
                Spacing = new Size(0, 5),
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
                            // new TableRow(),
                            new TableRow(
                                (_btnClearData = new LinkButton() {Text = "Reset Data"}),
                                (_cbExperimentalExec = new CheckBox()
                                    {
                                        Text = "Execute in Background", Enabled = true,
                                        ToolTip =
                                            "Executes model in a background thread such that Rhino does not freeze (experimental)."
                                    }
                                )
                            )
                        }
                    }
                }
            };
        }

        private void BindGui()
        {
            DataContext = _vm;
            _btnExecPython.Command = _vm.CommandExecuteModel;
            _btnInspectPython.Command = _vm.CommandInspectModel;
            _btnClearData.Command = _vm.CommandResetData;
            _cbExperimentalExec.BindDataContext(c => c.Checked,
                Binding.Property((CommonGenerateCodeViewModel m) => m.ExecuteInBackground));
        }
    }
}