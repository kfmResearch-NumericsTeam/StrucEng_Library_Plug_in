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
        private Scrollable _scrollable;
        private LinFeMainView _linFeMainView;
        public static System.Guid PanelId => typeof(MainView).GUID;

        public static MainView Instance { get; private set; }

        public MainView()
        {
            Instance = this;
            LoadUi();
        }

        public void LoadUi()
        {
            var tabs = new TabControl();
            var pageLinFe = new TabPage
            {
                Text = "LinFE Model",
                Content = _linFeMainView = new LinFeMainView(StrucEngLibPlugin.Instance.MainViewModel.LinFeMainVm)
            };
            var pageSm = new TabPage
            {
                Text = "Sandwich Model",
                Content = new Button()
                {
                    Text = "Test"
                }
            };
            tabs.Pages.Add(pageLinFe);
            tabs.Pages.Add(pageSm);
            Content = tabs;
        }

        public void DisposeUi()
        {
            _linFeMainView?.DisposeUi();
            Content.Dispose();
            Content = null;
            _linFeMainView = null;
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