using System;
using System.Diagnostics;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Layer;

namespace StrucEngLib
{
    /// <summary>
    /// Main Panel of the rhino plugin
    /// </summary>
    public class LinFeMainView : DynamicLayout
    {
        private Scrollable _scrollable;
        public static LinFeMainView Instance { get; private set; }

        public LinFeMainView(LinFeMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(LinFeMainViewModel vm)
        {
            RhinoApp.WriteLine("LinFeMainView init UI");
            _scrollable = new Scrollable
            {
                Content = new Button() { Text = "Linfe"}
            };
            
            // var v = new ListLayerView(vm);
            Content = new Button() { Text = "Linfe"};
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}