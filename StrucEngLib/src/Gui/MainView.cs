using System;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using StrucEngLib.Gui.LinFe;
using StrucEngLib.Gui.Settings;
using StrucEngLib.Gui.Sm;

namespace StrucEngLib.Gui
{
    /// <summary>
    /// Main Panel of the rhino plugin
    /// </summary>
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainView : Panel, IPanel
    {
        private LinFeMainView _linFeMainView;
        private SmMainView _smMainView;
        private SettingsMainView _settingsMainView;
        
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
            TabPage pageLinFe, pageSm, pageSettings;
            Content = new TabControl()
            {
                BackgroundColor = _bgColor,
                Pages =
                {
                    (pageLinFe = new TabPage
                    {
                        BackgroundColor = _bgColor,
                        Text = "LinFE Model",
                        Content = _linFeMainView =
                            new LinFeMainView(StrucEngLibPlugin.Instance.MainViewModel.LinFeMainVm)
                    }),
                    (pageSm = new TabPage
                    {
                        BackgroundColor = _bgColor,
                        Text = "Sandwich Model",
                        Content = _smMainView = new SmMainView(StrucEngLibPlugin.Instance.MainViewModel.SmMainVm),
                    }),
                    (pageSettings = new TabPage
                    {
                        BackgroundColor = _bgColor,
                        Text = "Settings",
                        Content = _settingsMainView = new SettingsMainView(StrucEngLibPlugin.Instance.MainViewModel.SettingsVm),
                    })
                }
            };
            pageSm.Click += (sender, args) => { StrucEngLibPlugin.Instance.MainViewModel.ReloadSandwich(); };
            pageLinFe.Click += (sender, args) => { StrucEngLibPlugin.Instance.MainViewModel.ReloadLinFe(); };
            pageSettings.Click += (sender, args) => { StrucEngLibPlugin.Instance.MainViewModel.ReloadSettings(); };
        }

        public void DisposeUi()
        {
            _linFeMainView?.DisposeUi();
            _smMainView?.DisposeUi();
            _settingsMainView?.DisposeUi();
            Content.Dispose();
            Content = null;
            _linFeMainView = null;
            _smMainView = null;
            _settingsMainView = null;
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