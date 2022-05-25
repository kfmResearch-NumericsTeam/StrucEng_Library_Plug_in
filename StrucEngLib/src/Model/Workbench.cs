using System;
using System.Collections.Generic;

namespace StrucEngLib.Model
{
    public class Workbench
    {
        /// <summary> Version for serialization purpose </summary>
        public string Version { get; } = "1";
        public List<Layer> Layers { get; } = new List<Layer>();
        public List<Load> Loads { get; } = new List<Load>();
        public List<Step> Steps { get; } = new List<Step>();
    }
}