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
    class InspectPythonDialog : Dialog<DialogResult>
    {
        public string Source { get; set; }

        public InspectPythonDialog(string source)
        {
            this.Source = source;
            Padding = new Padding(5);
            Resizable = true;
            Result = DialogResult.Cancel;
            WindowStyle = WindowStyle.Default;
            Title = "Inspect and Modify Generated Python Code";

            TextArea textArea = new TextArea()
            {
                Text = source
            };

            DefaultButton = new Button {Text = "OK"};
            DefaultButton.Click += (sender, e) =>
            {
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
                    new TableRow(TableLayout.AutoSized(DefaultButton))
                }
            };
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            this.RestorePosition();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.SavePosition();
            base.OnClosing(e);
        }
    }
}