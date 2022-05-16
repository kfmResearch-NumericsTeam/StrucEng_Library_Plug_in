using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Rhino.Runtime.RhinoAccounts;

namespace StrucEngLib
{
    /// <summary>Error messages vm</summary>
    public class ErrorViewModel : ViewModelBase
    {
        private string _message = "";

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public void ShowException(string info, Exception e)
        {
            StringBuilder b = new StringBuilder();
            b.Append($"An Exception occured. This is likely a bug, try to reproduce the bug and file an issue: {StrucEngLibPlugin.Website} \n\n");
            b.Append($"Version: {StrucEngLibPlugin.Version} \n");
            b.Append($"Info: {info} \n\n");
            b.Append($"Exception: {e.GetType().ToString()} \n");
            b.Append($"Message: {e.Message} \n");
            b.Append($"Source: {e.Source} \n");
            b.Append($"Stacktrace: {e.StackTrace} \n");
            ShowMessage(b.ToString(), false);
        }

        public void ShowMessage(string m, bool enumerate = true)
        {
            // XXX: For now a simple show text dialog is enough
            ShowMessages(new List<string>() {m});
        }

        public void ShowMessages(List<string> ms, bool enumerate = true)
        {
            StringBuilder b = new StringBuilder();
            b.Append("The following messages occured: \n");
            foreach (var m in ms)
            {
                if (enumerate)
                {
                    b.Append("\t- " + m + "\n");
                }
                else
                {
                    b.Append(m + "\n");
                }
            }

            Message = b.ToString();
            Rhino.UI.Dialogs.ShowTextDialog(Message, "Messages");
            Message = "";
        }
    }
}