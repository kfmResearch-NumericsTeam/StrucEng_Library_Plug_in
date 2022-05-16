using System;
using System.Collections.Generic;
using Rhino.Runtime.InteropWrappers;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>Validation of model</summary>
    public class ModelValidator
    {
        /// <summary>
        /// Returns a list of validation messages. If list has count = 0, no errors occured
        /// </summary>
        public List<string> ValidateModel(Workbench model)
        {
            var msgs = new List<string>();
            if (model.Layers == null || model.Layers.Count == 0)
            {
                msgs.Add("No Layers added");
            }
            else
            {
                foreach (var layer in model.Layers)
                {
                    if (layer.LayerType == LayerType.ELEMENT)
                    {
                        var element = (Element) layer;
                        if (element.ElementMaterialElastic == null)
                        {
                            msgs.Add("No Material Elastic for Layer " + element.GetName());
                        }
                        else
                        {
                            var matEl = element.ElementMaterialElastic;
                            if (String.IsNullOrEmpty(matEl.E)) msgs.Add("E in Material Elastic can't be empty");
                            if (String.IsNullOrEmpty(matEl.P)) msgs.Add("P in Material Elastic can't be empty");
                            if (String.IsNullOrEmpty(matEl.V)) msgs.Add("V in Material Elastic can't be empty");
                        }

                        if (element.ElementShellSection == null)
                        {
                            msgs.Add("No Shell Section for Layer " + element.GetName());
                        }
                        else
                        {
                            var shellSec = element.ElementShellSection;
                            if (String.IsNullOrEmpty(shellSec.Thickness))
                                msgs.Add("Thickness in Shell Section can't be empty");
                        }
                    }

                    if (layer.LayerType == LayerType.SET)
                    {
                        var set = (Set) layer;
                        if (set.SetDisplacementType == SetDisplacementType.NONE)
                        {
                            msgs.Add("No Displacement for Layer " + set.GetName());
                            if (set.SetGeneralDisplacement != null)
                            {
                                msgs.Add("Invalid state: general displacement set for displacement which has no data" +
                                         set.GetName());
                            }
                        }

                        if (set.SetDisplacementType == SetDisplacementType.GENERAL &&
                            set.SetGeneralDisplacement == null)
                        {
                            msgs.Add("General displacement cannot have no data set: " + set.GetName());
                        }
                    }
                }
            }

            foreach (var load in model.Loads)
            {
                if (load.Layers == null || load.Layers.Count == 0)
                {
                    msgs.Add("No Layers in load " + load.LoadType);
                }

                if (load.LoadType == LoadType.Area)
                {
                    var a = (LoadArea) load;
                    // XXX: No validation for now
                }

                if (load.LoadType == LoadType.Gravity)
                {
                    var a = (LoadGravity) load;
                    if (!isDouble(a.X)) msgs.Add($"X: {a.X} not numeric for gravity load");
                    if (!isDouble(a.Y)) msgs.Add($"Y: {a.Y} not numeric for gravity load");
                    if (!isDouble(a.Z)) msgs.Add($"Z: {a.Z} not numeric for gravity load");
                }
            }

            HashSet<string> steps = new HashSet<string>();
            foreach (var step in model.Steps)
            {
                if (String.IsNullOrEmpty(step.Order))
                {
                    continue;
                }

                try
                {
                    float.Parse(step.Order);
                    steps.Add(step.Order);
                }
                catch (Exception e)
                {
                    msgs.Add($"Order '{step.Order}' of '{step.GetSummary()}' is not numeric");
                }
            }

            foreach (var a in model.AnalysisSettings)
            {
                if (!steps.Contains(a.StepId))
                {
                    msgs.Add($"Analysis for Step '{a.StepId}' was not defined as a valid step. " +
                             $"Assign step or exclude from output");
                }
            }

            // XXX: We currently don't return success flag
            return msgs;
        }

        protected bool isDouble(string v)
        {
            double _;
            return double.TryParse(v, out _);
        }
    }
}