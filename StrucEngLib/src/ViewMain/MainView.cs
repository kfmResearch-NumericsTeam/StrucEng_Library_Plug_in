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
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainView : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(MainView).GUID;

        public static MainView Instance { get; private set; }

        private Scrollable _scrollable;

        public MainView()
        {
            Instance = this;
            LoadUi();
        }

        public void LoadUi()
        {
            var vm = StrucEngLibPlugin.Instance.MainViewModel;
            var v = new ListLayerView(vm);

            Content = (_scrollable = new Scrollable
            {
                Content = v
            });
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }

        public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
        {
        }

        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
        {
        }

        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
        {
        }
    }
}