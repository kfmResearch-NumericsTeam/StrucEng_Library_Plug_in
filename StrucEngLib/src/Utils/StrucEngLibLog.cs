using System;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json;
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

        private string Serialize(object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            }
            catch (Exception e)
            {
                return o.ToString();
            }
        }

        private void ProcessArgs(ref object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (!(args[i] is string))
                {
                    args[i] = Serialize(args[i]);
                }
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            ProcessArgs(ref args);
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
            ProcessArgs(ref args);
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