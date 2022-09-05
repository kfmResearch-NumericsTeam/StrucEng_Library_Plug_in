using System;
using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Utils
{
    /// <summary>
    /// Utility to scroll a parent on a mouse wheel event
    /// This is a workaround to enable scroll of container panel.
    /// </summary>
    public class ScrollHelper
    {
        public static void ScrollParent(Control c)
        {
            const int jump = 40;

            if (c == null) return;
            c.MouseWheel += (sender, args) =>
            {
                if (c.Parents == null) return;
                foreach (var p in c.Parents)
                {
                    if (p is Scrollable s)
                    {
                        var j = (int) (s.ScrollPosition.Y - (args.Delta.Height * jump));
                        j = Math.Max(0, j);
                        s.ScrollPosition = new Point(s.ScrollPosition.X, j);
                    }
                }

                args.Handled = true;
            };
        }
    }
}