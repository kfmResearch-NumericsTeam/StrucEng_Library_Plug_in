using System;
using System.Collections.Generic;
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
                        if (set.SetDisplacement == null)
                        {
                            msgs.Add("No Displacement for Layer " + set.GetName());
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
            }

            foreach (var step in model.Steps)
            {
                if (String.IsNullOrEmpty(step.Order))
                {
                    continue;
                }

                try
                {
                    float.Parse(step.Order);
                }
                catch (Exception e)
                {
                    msgs.Add($"Order '{step.Order}' of '{step.GetSummary()}' is not numeric");
                }
            }

            // XXX: We currently don't return success flag
            return msgs;
        }
    }
}