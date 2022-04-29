using Eto.Forms;
using Rhino.UI;

namespace CodeGenerator
{
    /// <summary>
    /// Main Panel of the rhino plugin
    /// </summary>
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainViewPanel : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(MainViewPanel).GUID;

        public MainViewPanel()
        {
            var listLayerVm = new ListLayerViewModel();
            var detailLayerVm = new LayerDetailsViewModel(listLayerVm);
            
            ListLayerView view = new ListLayerView(listLayerVm, detailLayerVm);
            Content = new Scrollable {Content = view};
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