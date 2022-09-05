using Rhino;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Commands;

namespace StrucEngLib.Utils
{
    /// <summary>
    /// Subscript Label
    /// </summary>
    public class SubscriptLabel : Panel
    {
        public Label Label => _label;
        public Label LabelSubscript => _labelSubscript;

        private readonly Label _label;
        private readonly Label _labelSubscript;

        public string Text
        {
            get { return _label.Text; }
            set { _label.Text =  value; }
        }

        public string TextSubscript
        {
            get { return _labelSubscript.Text; }
            set { _labelSubscript.Text = value; }
        }

        public SubscriptLabel()
        {
            var s = new Label().Font.Size;
            var subs = (float) (s * 0.8);
            _label = new Label()
            {
                Font = new Font(FontFamilies.Sans, s, FontStyle.None)
            };
            _labelSubscript = new Label()
            {
                Font = new Font(FontFamilies.Sans, subs, FontStyle.None),
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            Content = new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Spacing = 0,
                Items =
                {
                    new DynamicLayout()
                    {
                        Rows = {_label},
                        Padding = new Padding(0, 0, 0, 0),
                    },
                    new DynamicLayout()
                    {
                        Padding = new Padding(1, 4, 0, 0),
                        Rows = {_labelSubscript}
                    }
                }
            };
        }
    }
}