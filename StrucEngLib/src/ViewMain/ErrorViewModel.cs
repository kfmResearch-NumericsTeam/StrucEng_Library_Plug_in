using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Runtime.RhinoAccounts;

namespace StrucEngLib
{
    /// <summary>Error messages vm</summary>
    public class ErrorViewModel: ViewModelBase
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

        public void ShowMessage(string m)
        {
            // XXX: For now a simple show text dialog is enough
            ShowMessages(new List<string>() {m});
        }
        
        public void ShowMessages(List<string> ms)
        {
            StringBuilder b = new StringBuilder();
            b.Append("The following messages occured: \n");
            foreach (var m in ms)
            {
                b.Append("\t- " + m + "\n");
            }

            Message = b.ToString();
            Rhino.UI.Dialogs.ShowTextDialog(Message, "Messages");
            Message = "";
        }
    }
}