using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary></summary>
    public class ShowImageForm: Form
    {

        public ShowImageForm()
        {
            Maximizable = true;
            Minimizable = true;
            Padding = new Padding(10);
            Resizable = true;
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.Default;
            Title = "Image Visualization";
            Content = new ImageView()
            {
                Size = new Size(500, 500),
                // Image = Rhino.UI.EtoExtensions.ToEto()
            };
        }
    }
}