using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Gui.Sm
{
    /// <summary>About Sandwich Model</summary>
    public class AboutSmView: DynamicLayout
    {
        public AboutSmView()
        {
            Padding = new Padding(5, 5, 5, 10);
            Spacing = new Size(5, 1);
            AddRow(new Label()
            {
                Text = "Description of the Sandwich Model here."
            });
        }
        
    }
}