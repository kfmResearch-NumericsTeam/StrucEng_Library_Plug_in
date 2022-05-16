using System;
using Eto.Forms;
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
            MainViewModel vm = new MainViewModel();
            Content = new Scrollable {Content = new ListLayerView(vm)};
            // Content = new Scrollable {Content = };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                vm.ErrorVm.ShowException("Something went wrong, we caught an unhandled exception. " +
                                         "This is a bug. This will leave the application in an inconsistent state",
                    (Exception) args.ExceptionObject);
            };
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