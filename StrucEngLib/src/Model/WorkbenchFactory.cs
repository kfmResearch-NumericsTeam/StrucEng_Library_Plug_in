using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Rhino;

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

                foreach (var l in bench.Layers)
                {
                    RhinoApp.WriteLine("Layer deserial: {0}", l.GetName());
                }
            }
            catch (Exception e)
            {
                RhinoApp.WriteLine("Something went wrong wile reading the model: {0}", data);
                RhinoApp.WriteLine(e.Message);
                RhinoApp.WriteLine("We start off an empty model instead...");
            }

            return bench;
        }

        public string SerializeToString(Workbench data)
        {
            foreach (var l in data.Layers)
            {
                RhinoApp.WriteLine("Layer: {0}", l.GetName());
            }

            var s = JsonConvert.SerializeObject(data, _settings);
            RhinoApp.WriteLine("serialize: {0}", s);
            return s;
        }
    }
}