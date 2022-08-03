using System;
using System.Runtime.ConstrainedExecution;
using Rhino;

namespace StrucEngLib.Utils
{
    /// <summary>Logging singleton for sturcenglib</summary>
    public class StrucEngLibLog
    {
        private static readonly Lazy<StrucEngLibLog> lazy =
            new Lazy<StrucEngLibLog>(() => new StrucEngLibLog());

        public static StrucEngLibLog Instance
        {
            get { return lazy.Value; }
        }

        private StrucEngLibLog()
        {
        }

        private String Tag(String suffix = null) => suffix == null ? "[strucenglib] " : $"[strucenglib/{suffix}] ";

        public void WriteLine(string format, params object[] args)
        {
            WriteInternal(Tag() + String.Format(format, args));
        }

        public void WriteLine(string format)
        {
            WriteInternal(Tag() + format);
        }

        public void WriteTaggedLine(string tag, string format)
        {
            WriteInternal(Tag(tag) + format);
        }

        public void WriteTaggedLine(string tag, string format, params object[] args)
        {
            WriteInternal(Tag(tag) + String.Format(format, args));
        }
        
        private void WriteInternal(string msg)
        {
            try
            {
                Rhino.RhinoApp.WriteLine(msg);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(msg);
            }
        }
    }
}