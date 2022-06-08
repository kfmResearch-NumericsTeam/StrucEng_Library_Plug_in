using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Rhino.Runtime.RhinoAccounts;
using StrucEngLib.Utils;

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
            b.Append(
                $"An Exception occured. This is likely a bug. Save this message, " +
                $"try to reproduce the bug and file an issue: {StrucEngLibPlugin.Website} \n\n");
            b.Append($"Version: {StrucEngLibPlugin.Version} \n");
            b.Append($"Info: {info} \n\n");
            b.Append($"Exception: {e.GetType().ToString()} \n");
            b.Append($"Message: {e.Message} \n");
            b.Append($"Source: {e.Source} \n");
            b.Append($"Stacktrace: {e.StackTrace} \n");
            b.Append("\n Context:\n");
            try
            {
                b.Append(StringUtils.ToJson(StrucEngLibPlugin.Instance.MainViewModel));
                b.Append("\n Model:\n");
                b.Append(StringUtils.ToJson(StrucEngLibPlugin.Instance.MainViewModel.Workbench));
            }
            catch (Exception)
            {
                // XXX: Ignore errors caused by serialization error context
            }

            ShowMessage(b.ToString(), false);
        }

        public void DebugMessage(params object[] values)
        {
            StringBuilder b = new StringBuilder();
            foreach (var o in values)
            {
                b.Append(StringUtils.ToJson(o));
                b.Append("\n");
            }

            ShowMessages(new List<string>() {b.ToString()}, false);
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

        public void ShowMessages(ErrorMessageContext ctx, bool enumerate = true)
        {
            StringBuilder b = new StringBuilder();
            b.Append("The following messages occured: \n\n");
            var infoMessages = ctx.GetByType(MessageType.Info);
            if (infoMessages != null && infoMessages.Count > 0)
            {
                b.Append("Info Messages: \n");
                foreach (var m in infoMessages)
                {
                    if (enumerate)
                    {
                        b.Append("\t- " + m.Text + "\n");
                    }
                    else
                    {
                        b.Append(m.Text + "\n");
                    }
                }
            }

            var warnMessages = ctx.GetByType(MessageType.Warning);
            if (warnMessages != null && warnMessages.Count > 0)
            {
                b.Append("Warning Messages: \n");
                foreach (var m in warnMessages)
                {
                    if (enumerate)
                    {
                        b.Append("\t- " + m.Text + "\n");
                    }
                    else
                    {
                        b.Append(m.Text + "\n");
                    }
                }
            }

            var errorMsgs = ctx.GetByType(MessageType.Error);
            if (errorMsgs != null && errorMsgs.Count > 0)
            {
                b.Append("Error Messages: \n");
                foreach (var m in errorMsgs)
                {
                    if (enumerate)
                    {
                        b.Append("\t- " + m.Text + "\n");
                    }
                    else
                    {
                        b.Append(m.Text + "\n");
                    }
                }
            }


            Message = b.ToString();
            Rhino.UI.Dialogs.ShowTextDialog(Message, "Messages");
            Message = "";
        }
    }
}