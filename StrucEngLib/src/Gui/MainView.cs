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
    /// Main Panel of the rhino plugin
    /// </summary>
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainView : Panel, IPanel
    {
        private LinFeMainView _linFeMainView;
        private SmMainView _smMainView;
        private Color _bgColor = new Label().BackgroundColor;

        public static Guid PanelId => typeof(MainView).GUID;
        public static MainView Instance { get; private set; }

        public MainView()
        {
            Instance = this;
            LoadUi();
        }

        public void LoadUi()
        {
            var tabs = new TabControl();
            tabs.BackgroundColor = _bgColor;
            var pageLinFe = new TabPage
            {
                BackgroundColor = _bgColor,
                Text = "LinFE Model",
                Content = _linFeMainView = new LinFeMainView(StrucEngLibPlugin.Instance.MainViewModel.LinFeMainVm)
            };
            var pageSm = new TabPage
            {
                BackgroundColor = _bgColor,
                Text = "Sandwich Model",
                Content = _smMainView = new SmMainView(StrucEngLibPlugin.Instance.MainViewModel.SmMainVm),
            };

            // XXX: Whenever we access sandwich we sync model state into vms;
            // We ensure that we only depend on state defined in model and not view model of LinFe
            pageSm.Click += (sender, args) =>
            {
                StrucEngLibPlugin.Instance.MainViewModel.LinFeMainVm.UpdateModel();
                StrucEngLibPlugin.Instance.MainViewModel.SmMainVm.UpdateViewModel();
            };
            pageLinFe.Click += (sender, args) =>
            {
                StrucEngLibPlugin.Instance.MainViewModel.SmMainVm.UpdateModel();
                StrucEngLibPlugin.Instance.MainViewModel.LinFeMainVm.UpdateViewModel();
            };

            tabs.Pages.Add(pageLinFe);
            tabs.Pages.Add(pageSm);
            Content = tabs;
        }

        public void DisposeUi()
        {
            _linFeMainView?.DisposeUi();
            _smMainView?.DisposeUi();
            Content.Dispose();
            Content = null;
            _linFeMainView = null;
            _smMainView = null;
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