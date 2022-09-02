using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>About LinFe</summary>
    public class AboutLinFeView: DynamicLayout
    {
        public AboutLinFeView()
        {
            Padding = new Padding(5, 5, 5, 10);
            Spacing = new Size(5, 1);
            AddRow(new Label()
            {
                Text = "Description of the Linear Finite Element Model here."
            });
        }
        
    }
}