using Eto.Forms;
using Rhino.UI;

namespace CodeGenerator
{
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C8")]
    public class MainViewPanel : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(MainViewPanel).GUID;

        public MainViewPanel()
        {
            var viewModel = new MainViewModel();
            MainView view = new MainView(viewModel);
            
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