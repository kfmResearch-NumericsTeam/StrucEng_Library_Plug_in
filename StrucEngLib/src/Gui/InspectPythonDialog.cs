using Eto.Drawing;
using Eto.Forms;
using Rhino;

namespace StrucEngLib.Gui
{
    /// <summary>
    /// Dialog to inspect generated python code
    /// </summary>
    public class InspectPythonDialog : Dialog<DialogResult>
    {
        public static int STATE_CLOSE = 0;
        public static int STATE_EXEC = 1;
        public string Source { get; set; }

        public int State = STATE_CLOSE;

        private Button _execButton;
        
        private Button _openEditor;

        public InspectPythonDialog(string source)
        {
            this.Source = source;
            // Padding = new Padding(5);
            Resizable = true;
            AutoSize = true;
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.Default;
            Size = new Size(1000, 800);

            Result = DialogResult.Cancel;
            WindowStyle = WindowStyle.Default;

            Title = "Generated code";

            _execButton = new Button()
            {
                Text = "Execute"
            };
            
            _openEditor = new Button()
            {
                Text = "Open Editor",
                ToolTip = "Python code is copied into clipboard"
                
            }; 

            TextArea textArea = new TextArea()
            {
                Text = source
            };
            DefaultButton = new Button {Text = "Close"};
            DefaultButton.Click += (sender, e) =>
            {
                State = STATE_CLOSE;
                Source = textArea.Text;
                Close(DialogResult.Ok);
            };

            _execButton.Click += (sender, e) =>
            {
                State = STATE_EXEC;
                Source = textArea.Text;
                Close(DialogResult.Ok);
            };

            _openEditor.Click += (sender, args) =>
            {
                Clipboard.Instance.Text = textArea.Text;
                RhinoApp.ExecuteCommand(RhinoDoc.ActiveDoc, "EditPythonScript");
                Size = new Size(100, -1);
            };

            Content = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow {ScaleHeight = true, Cells = {new TableCell(textArea, true)}},
                    new TableRow(TableLayout.AutoSized(new TableLayout()
                    {
                        Spacing = new Size(10, 10),
                        Padding = new Padding()
                        {
                            Top = 10,
                            Bottom = 10,
                        },
                        Rows =
                        {
                            new TableRow(DefaultButton, _execButton, _openEditor),
                        }
                    }))
                }
            };
            KeyDown += (sender, args) => KeyDownHandel(sender, args);
        }

        private void KeyDownHandel(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}