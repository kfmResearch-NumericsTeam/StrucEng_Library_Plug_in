using System;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Utils
{
    /// <summary>Simple dialog to show text content</summary>
    class MessageDialog : Form
    {
        public MessageDialog(string title, string content)
        {
            Maximizable = true;
            Minimizable = true;
            Padding = new Padding(5);
            Resizable = true;
            ShowInTaskbar = true;
            Title = title;
            Width = 700;
            Height = 700;
            var c = Screen.DisplayBounds.Center;
            Location = new Point(Math.Max(0, (int) (c.X - 500)), (int) c.Y);

            WindowStyle = WindowStyle.Default;
            var info = new TableLayout
            {
                Padding = new Padding(5, 10, 5, 5),
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(new TableCell(new TextArea() {Text = content}, true))
                }
            };

            var controls = new TableLayout
            {
                Padding = new Padding(5, 10, 5, 5),
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(null,
                        TableLayout.AutoSized(new Button {Text = "Ok"}),
                        TableLayout.AutoSized(new Button {Text = "Cancel"}),
                        null)
                }
            };

            Content = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(info),
                    new TableRow(controls),
                }
            };
        }
    }
}