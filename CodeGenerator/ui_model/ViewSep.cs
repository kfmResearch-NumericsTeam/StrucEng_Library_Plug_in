using Rhino;
using Rhino.UI;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Commands;

namespace CodeGenerator.Views
{
    public class ViewSeparator : Panel
    {
        readonly Label m_label;
        readonly Divider m_divider;

        public string Text
        {
            get { return m_label.Text; }
            set { m_label.Text = value; }
        }

        public Color Color
        {
            get { return m_divider.Color; }
            set { m_divider.Color = value; }
        }

        public ViewSeparator()
        {
            m_label = new Label();
            m_divider = new Divider {Color = Colors.DarkGray};

            Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Spacing = 2,
                Items =
                {
                    m_label,
                    new StackLayoutItem(m_divider, true)
                }
            };
        }
    }

    internal class Divider : Eto.Forms.Drawable
    {
        private Eto.Drawing.Color m_color;

        public Eto.Drawing.Color Color
        {
            get { return m_color; }
            set
            {
                if (m_color == value)
                {
                    return;
                }

                m_color = value;
                Invalidate();
            }
        }

        public Orientation Orientation => Width < Height
            ? Orientation.Vertical
            : Orientation.Horizontal;

        public Divider()
        {
            m_color = Colors.DarkGray;
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