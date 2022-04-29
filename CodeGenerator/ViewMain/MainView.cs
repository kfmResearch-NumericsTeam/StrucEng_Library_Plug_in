using Eto.Forms;
using Rhino.UI;

namespace CodeGenerator
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
            MainViewModel vm = new MainViewModel();
            Content = new Scrollable {Content = new ListLayerView(vm)};
            // Content = new Scrollable {Content = };
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