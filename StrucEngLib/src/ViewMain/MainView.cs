using System;
using Eto.Forms;
using Rhino;
using Rhino.UI;

namespace StrucEngLib
{
    /// <summary>
    /// Main Panel of the rhino plugin
    /// </summary>
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainView : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(MainView).GUID;

        public MainView()
        {
            LoadUi();
        }

        public void LoadUi()
        {
            var vm = StrucEngLibPlugin.Instance.MainViewModel;
            Content = new Scrollable {Content = new ListLayerView(vm)};
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