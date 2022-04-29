using System;
using System.ComponentModel;
using System.Net.Mime;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.UI;


namespace CodeGenerator
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

        public InspectPythonDialog(string source)
        {
            this.Source = source;
            Padding = new Padding(5);
            Resizable = true;
            Result = DialogResult.Cancel;
            WindowStyle = WindowStyle.Default;
            
            Title = "Inspect and Modify Generated Python Code";

            _execButton = new Button()
            {
                Text = "Execute Code"
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
                        Rows =
                        {
                            new TableRow(DefaultButton, _execButton),
                        }
                    }))
                }
            };
        }
    }
}