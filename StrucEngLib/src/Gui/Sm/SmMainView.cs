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
    public class SmMainView : Scrollable
    {
        public static SmMainView Instance { get; private set; }

        public SmMainView(SmMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(SmMainViewModel vm)
        {
            BackgroundColor = new Label().BackgroundColor;
            Content = new Label()
            {
                Text = "Sandwich View"
            };
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}