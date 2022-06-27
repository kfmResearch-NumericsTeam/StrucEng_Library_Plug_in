using Rhino;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Commands;

namespace StrucEngLib.Views
{
    /// <summary>
    /// Class to draw a text and a separation line next to it
    /// </summary>
    public class ViewSeparator : Panel
    {
        public Label Label => _label;

        private readonly Label _label;
        readonly Divider _divider;

        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        public Color Color
        {
            get { return _divider.Color; }
            set { _divider.Color = value; }
        }


        public ViewSeparator()
        {
            _label = new Label();
            _divider = new Divider {Color = Colors.DarkGray};

            Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Spacing = 2,
                Items =
                {
                    _label,
                    new StackLayoutItem(_divider, true)
                }
            };
        }
    }

    internal class Divider : Eto.Forms.Drawable
    {
        private Eto.Drawing.Color _color;

        public Eto.Drawing.Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                {
                    return;
                }

                _color = value;
                Invalidate();
            }
        }

        public Orientation Orientation => Width < Height
            ? Orientation.Vertical
            : Orientation.Horizontal;

        public Divider()
        {
            _color = Colors.DarkGray;
            Size = new Size(3, 3);
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void OnLoadComplete(System.EventArgs e)
        {
            base.OnLoadComplete(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var middle = new PointF(Size / 2);
            e.Graphics.FillRectangle(
                Color,
                Orientation == Orientation.Horizontal
                    ? new RectangleF(0f, middle.Y, ClientSize.Width, 1)
                    : new RectangleF(middle.Y, 0f, 1, ClientSize.Height));
        }
    }
}