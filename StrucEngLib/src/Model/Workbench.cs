using System;
using System.Collections.Generic;
using System.Linq;
using StrucEngLib.Model.Sm;

namespace StrucEngLib.Model
{
    /// <summary>
    /// Main application model
    /// </summary>
    public class Workbench
    {
        public string Version { get; } = "1";

        public List<Layer> Layers { get; } = new List<Layer>();
        public List<Load> Loads { get; } = new List<Load>();
        public List<Step> Steps { get; } = new List<Step>();

        /// <summary> The path to the generated file </summary>
        public string FileName { get; set; }
        
        /// <summary> Setting to run code in background </summary>
        public bool ExecuteInBackground { get; set; }

        public SandwichModel SandwichModel { get; set; }

        public List<Element> Elements()
        {
            return Layers.Where(l => l.LayerType == LayerType.ELEMENT).ToList().Select(l => l as Element).ToList();
        }

        public List<Set> Sets()
        {
            return Layers.Where(l => l.LayerType == LayerType.SET).ToList().Select(l => l as Set).ToList();
        }
    }
}