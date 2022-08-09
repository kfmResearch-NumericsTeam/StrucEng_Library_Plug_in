using System;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI.Forms;

namespace StrucEngLib.Utils
{
    /// <summary>
    /// Simple dialog to show when Rhino freezes during code execution.
    /// </summary>
    class FreezeDialog : Form
    {
        public FreezeDialog()
        {
            Maximizable = false;
            Minimizable = false;
            Padding = new Padding(5);
            Resizable = false;
            ShowInTaskbar = false;
            Topmost = true;
            Title = "Please wait";
            var c = Screen.DisplayBounds.Center;
            Location = new Point(Math.Max(0, (int) (c.X - 500)), (int) c.Y);
            
            WindowStyle = WindowStyle.Default;
            var info = new TableLayout
            {
                Padding = new Padding(5, 10, 5, 5),
                Spacing = new Size(5, 5),
                Rows =
                {
                    new TableRow(null,
                        new Label {Text = "StrucEngLib is executing code. Rhino freezes until this operation is completed. This may take some time. "},
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
                }
            };
        }
    }
}