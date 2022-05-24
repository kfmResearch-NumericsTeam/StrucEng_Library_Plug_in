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
            };
        }

        public static WorkbenchFactory Instance { get; private set; } = new WorkbenchFactory();

        public Workbench DeserializeFromString(string data)
        {
            RhinoApp.WriteLine("Deserialize model: {0}", data);
            if (string.IsNullOrWhiteSpace(data))
            {
                RhinoApp.WriteLine("Model data empty");
                return new Workbench();
            }

            Workbench bench = JsonConvert.DeserializeObject<Workbench>(data, _settings);
            if (bench == null)
            {
                bench = new Workbench();
            }
            foreach (var l in bench.Layers)
            {
                RhinoApp.WriteLine("Layer deserial: {0}", l.GetName());
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