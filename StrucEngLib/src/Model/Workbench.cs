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
        public List<AnalysisSetting> AnalysisSettings { get; } = new List<AnalysisSetting>();

        public override string ToString()
        {
            return $"{nameof(Layers)}: {Layers}, {nameof(Loads)}: {Loads}," +
                   $" {nameof(Steps)}: {Steps}, {nameof(AnalysisSettings)}: {AnalysisSettings}";
        }

        /*
         * Given A workbench, group steps according to their order (float).
         * Result is a List of steps belonging to the same order-id, ordered by order-id
         */
        public SortedDictionary<float, List<Model.Step>> GroupSteps()
        {
            var steps = new SortedDictionary<float, List<Model.Step>>();
            foreach (var step in Steps)
            {
                try
                {
                    var order = float.Parse(step.Order);
                    if (String.IsNullOrEmpty(step.Order)) continue;
                    if (steps.ContainsKey(order))
                    {
                        steps[order].Add(step);
                    }
                    else
                    {
                        steps.Add(order, new List<Model.Step>() {step});
                    }
                }
                catch (Exception)
                {
                    // XXX: We ignore invalid step numbers
                }
            }

            return steps;
        }
    }
}