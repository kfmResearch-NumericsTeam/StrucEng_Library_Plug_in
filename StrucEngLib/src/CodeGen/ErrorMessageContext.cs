using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StrucEngLib
{
    /// <summary>Represents a validation message</summary>
    public class ErrorMessage
    {
        public string Text { get; set; }
        public MessageType Type { get; set; }

        public static ErrorMessage Info(string message)
        {
            return new ErrorMessage()
            {
                Type = MessageType.Info,
                Text = message
            };
        }

        public static ErrorMessage Warning(string message)
        {
            return new ErrorMessage()
            {
                Type = MessageType.Warning,
                Text = message
            };
        }

        public static ErrorMessage Error(string message)
        {
            return new ErrorMessage()
            {
                Type = MessageType.Error,
                Text = message
            };
        }
    }

    public class ErrorMessageContext
    {
        
        public string ContextDescription { get; set; }

        public List<ErrorMessage> Messages { get; private set; }

        public List<ErrorMessage> GetByType(MessageType type)
        {
            List<ErrorMessage> res = new List<ErrorMessage>();
            foreach (var m in Messages)
            {
                if (m.Type == type)
                {
                    res.Add(m);
                }
            }

            return res;
        }

        public ErrorMessageContext()
        {
            Messages = new List<ErrorMessage>();
        }

        public void AddInfo(string msg)
        {
            Messages.Add(ErrorMessage.Info(msg));
        }

        public void AddWarning(string msg)
        {
            Messages.Add(ErrorMessage.Warning(msg));
        }

        public void AddError(string msg)
        {
            Messages.Add(ErrorMessage.Error(msg));
        }
    }

    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
}