using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>
    /// Utility to scroll a parent on a mouse wheel event
    /// This is a workaround to enable scroll of container panel.
    /// </summary>
    public class ScrollHelper
    {
        public static void ScrollParent(Control c)
        {
            const int jump = 20;

            if (c == null) return;
            c.MouseWheel += (sender, args) =>
            {
                if (c.Parents == null) return;
                foreach (var p in c.Parents)
                {
                    if (p is Scrollable s)
                    {
                        s.ScrollPosition = new Point(s.ScrollPosition.X,
                            (int) (s.ScrollPosition.Y - (args.Delta.Height * jump)));
                    }
                }

                args.Handled = true;
            };
        }
    }
}