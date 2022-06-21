using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Layer;

namespace StrucEngLib
{
    /// <summary>
    /// </summary>
    public class LinFeMainView : Scrollable
    {
        public static LinFeMainView Instance { get; private set; }

        public LinFeMainView(LinFeMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(LinFeMainViewModel vm)
        {
            BackgroundColor = new Label().BackgroundColor;
            var v = new ListLayerView(vm);
            Padding = new Padding()
            {
            };
            Content = v;
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}