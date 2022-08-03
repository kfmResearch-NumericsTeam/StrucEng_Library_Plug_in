using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib.Model
{
    /// <summary>Factory to build root model</summary>
    public class WorkbenchFactory
    {
        private JsonSerializerSettings _settings;

        protected WorkbenchFactory()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
        }

        public static WorkbenchFactory Instance { get; private set; } = new WorkbenchFactory();

        public Workbench DeserializeFromString(string data)
        {
            var bench = new Workbench();
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                {
                    return new Workbench();
                }

                bench = JsonConvert.DeserializeObject<Workbench>(data, _settings);
                if (bench == null)
                {
                    bench = new Workbench();
                }
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine("Something went wrong wile reading the model: {0}", data);
                StrucEngLibLog.Instance.WriteLine(e.Message);
                StrucEngLibLog.Instance.WriteLine("We start off an empty model instead...");
            }

            return bench;
        }

        public string SerializeToString(Workbench data)
        {
            var s = JsonConvert.SerializeObject(data, _settings);
            StrucEngLibLog.Instance.WriteLine("serialize: {0}", s);
            return s;
        }
    }
}