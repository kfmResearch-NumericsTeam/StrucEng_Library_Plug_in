using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>Utility to scroll a parent on a mouse wheel event</summary>
    public class ScrollHelper
    {
        public static void ScrollParent(Control c)
        {
            if (c == null) return;
            c.MouseWheel += (sender, args) =>
            {
                if (c.Parents == null) return;
                foreach (var p in c.Parents)
                {
                    if (p is Scrollable s)
                    {
                        s.ScrollPosition = new Point(args.Location.X);
                    }
                }
            };
        }
    }
}