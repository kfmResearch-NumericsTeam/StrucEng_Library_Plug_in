using Rhino.UI;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui
{
    /// <summary>Clears all user data</summary>
    public class ExecClearModelData : CommandBase
    {
        private readonly ClearState _clearState;

        public enum ClearState
        {
            ALL,
            SANDWICH
        }

        public ExecClearModelData(ClearState clearState = ClearState.ALL)
        {
            _clearState = clearState;
        }

        public override void Execute(object parameter)
        {
            var res = Dialogs.ShowMessage("Do you want to delete all associated data from StrucEngLib?", "Delete data",
                ShowMessageButton.YesNo, ShowMessageIcon.Warning);
            if (res == ShowMessageResult.Yes)
            {
                if (_clearState == ClearState.ALL)
                {
                    MainView.Instance.DisposeUi();
                    StrucEngLibPlugin.Instance.ResetData();
                    MainView.Instance.LoadUi();
                }
                else
                {
                    MainView.Instance.DisposeUi();
                    StrucEngLibPlugin.Instance.ResetSandwichData();
                    MainView.Instance.LoadUi();
                }
            }
        }
    }
}