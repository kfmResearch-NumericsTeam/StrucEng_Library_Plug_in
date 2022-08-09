using System;
using System.Collections.Generic;
using System.Linq;
using Rhino;
using Rhino.Runtime.InteropWrappers;
using StrucEngLib;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>Validation of model</summary>
    public class LinFeModelValidator
    {
        public ErrorMessageContext ValidateModel(Workbench model)
        {
            var ctx = new ErrorMessageContext()
            {
                ContextDescription = "LinFe Model"
            };
            if (model.Layers == null || model.Layers.Count == 0)
            {
                ctx.AddWarning("No Layers added");
            }
            else
            {
                ValidateLayers(model, ctx);
            }

            ValidateLoads(model, ctx);
            ValidateSteps(model, ctx);
            ValidateAnalysisSettings(model, ctx);
            return ctx;
        }

        private void ValidateAnalysisSettings(Workbench model, ErrorMessageContext ctx)
        {
            model.Steps.ForEach(step =>
            {
                if (step.Setting != null && step.Setting.Include)
                {
                    var s = step.Setting;
                    if (s.AnySettingEnabled && step.ContainsAnyStepTypeSetNested())
                    {
                        ctx.AddWarning(
                            $"Step {step.Order} contains a set and has analysis settings enabled. " +
                            $"This may cause an error.");
                    }
                }
            });
        }

        private void ValidateLayers(Workbench model, ErrorMessageContext ctx)
        {
            ValidateLayerNames(model.Layers, ctx);

            foreach (var layer in model.Layers)
            {
                if (layer.LayerType == LayerType.ELEMENT)
                {
                    var name = layer.GetName();
                    var element = (Element) layer;
                    if (element.ElementMaterialElastic == null)
                    {
                        ctx.AddError("No Material Elastic for Layer " + element.GetName());
                    }
                    else
                    {
                        var matEl = element.ElementMaterialElastic;
                        if (String.IsNullOrEmpty(matEl.E))
                        {
                            ctx.AddError("E in Material Elastic can't be empty: " + name);
                        }

                        if (String.IsNullOrEmpty(matEl.P))
                        {
                            ctx.AddError("P in Material Elastic can't be empty: " + name);
                        }

                        if (String.IsNullOrEmpty(matEl.V))
                        {
                            ctx.AddError("V in Material Elastic can't be empty: " + name);
                        }
                    }

                    if (element.ElementShellSection == null)
                    {
                        ctx.AddError(("No Shell Section for Layer " + element.GetName()));
                    }
                    else
                    {
                        var shellSec = element.ElementShellSection;
                        if (String.IsNullOrEmpty(shellSec.Thickness))
                        {
                            ctx.AddError("Thickness in Shell Section can't be empty: " + name);
                        }

                        if (!IsDouble(shellSec.Thickness))
                        {
                            ctx.AddError("Thickness in Shell Section must be numeric: " + name);
                        }
                        else
                        {
                            var t = double.Parse(shellSec.Thickness);
                            if (t == 0)
                            {
                                ctx.AddError("Thickness in Shell Section cant be 0: " + name);
                            }
                        }
                    }
                }

                if (layer.LayerType == LayerType.SET)
                {
                    var set = (Set) layer;
                    if (set.SetDisplacementType == SetDisplacementType.NONE)
                    {
                        ctx.AddError("No Displacement for Layer " + set.GetName());
                        if (set.SetGeneralDisplacement != null)
                        {
                            ctx.AddError(
                                "Invalid state: general displacement set for displacement which has no data" +
                                set.GetName());
                        }
                    }

                    if (set.SetDisplacementType == SetDisplacementType.GENERAL &&
                        set.SetGeneralDisplacement == null)
                    {
                        ctx.AddError("General displacement cannot have no set: " + set.GetName());
                    }
                }
            }
        }

        private void ValidateLayerNames(List<Model.Layer> layers, ErrorMessageContext ctx)
        {
            if (layers != null)
            {
                foreach (var l in layers)
                {
                    var name = l.GetName();
                    Rhino.DocObjects.RhinoObject[] rhobjs = RhinoDoc.ActiveDoc.Objects.FindByLayer(name);
                    if (rhobjs == null || rhobjs.Length < 1)
                    {
                        ctx.AddWarning($"No layer with name '{name}' found in active Rhino document.");
                    }
                }
            }
        }


        private void ValidateSteps(Workbench model, ErrorMessageContext ctx)
        {
            if (model.Steps == null || model.Steps.Count == 0)
            {
                ctx.AddInfo("No steps defined");
            }

            var hasAnalysis = false;
            var objectsInSteps = new HashSet<object>();
            var stepIds = new HashSet<string>();
            foreach (var step in model.Steps)
            {
                if (stepIds.Contains(step.Order))
                {
                    ctx.AddWarning(
                        $"Several steps contain the same step ID ({step.Order}). This will likely cause invalid state.");
                }
                else
                {
                    stepIds.Add(step.Order);
                }

                if (!IsInt(step.Order))
                {
                    ctx.AddWarning($"Step Id {step.Order} not numeric");
                }

                hasAnalysis = hasAnalysis || (step.Setting != null && step.Setting.Include == true);

                if (step.Entries == null || step.Entries.Count == 0)
                {
                    ctx.AddWarning($"Step {step.Order} contains no step entries");
                }
                else
                {
                    foreach (var entry in step.Entries)
                    {
                        if (objectsInSteps.Contains(entry.Value))
                        {
                            ctx.AddInfo($"Entries in step {step.Order} ambiguously assigned to other steps.");
                        }
                        else
                        {
                            objectsInSteps.Add(entry.Value);
                        }

                        if (entry.Value == null)
                        {
                            ctx.AddWarning($"Step {step.Order} contains an invalid entry (null)");
                        }
                    }
                }
            }

            if (!hasAnalysis)
            {
                ctx.AddInfo("No Analysis output defined");
            }
        }

        private void ValidateLoads(Workbench model, ErrorMessageContext ctx)
        {
            foreach (var load in model.Loads)
            {
                if (load.Layers == null || load.Layers.Count == 0)
                {
                    ctx.AddWarning("No Layers in load " + load.LoadType);
                }
                else
                {
                    ValidateLayerNames(load.Layers, ctx);
                    var hasElement = false;
                    var hasSet = false;
                    load.Layers.ForEach(loadLayer =>
                    {
                        if (!model.Layers.Contains(loadLayer))
                        {
                            ctx.AddWarning(
                                $"Load '{load.Description}' contains a layer '{loadLayer.GetName()}' " +
                                $"which was not added as a layer to the model.");
                        }

                        if (loadLayer.LayerType == LayerType.ELEMENT)
                        {
                            hasElement = true;
                        }
                        else if (loadLayer.LayerType == LayerType.SET)
                        {
                            hasSet = true;
                        }
                    });
                    if (hasElement && hasSet)
                    {
                        ctx.AddInfo($"Load '{load.Description}' contains sets and elements.");
                    }
                }

                if (load.LoadType == LoadType.Area)
                {
                    var a = (LoadArea) load;
                    if (!EmptyOrValidDouble(a.X)) ctx.AddError($"X: {a.X} not numeric for area load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Y)) ctx.AddError($"Y: {a.Y} not numeric for area load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Z)) ctx.AddError($"Z: {a.Z} not numeric for area load: " + a.Description);
                }

                if (load.LoadType == LoadType.Gravity)
                {
                    var a = (LoadGravity) load;
                    if (!EmptyOrValidDouble(a.X))
                        ctx.AddError($"X: {a.X} not numeric for gravity load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Y))
                        ctx.AddError($"Y: {a.Y} not numeric for gravity load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Z))
                        ctx.AddError($"Z: {a.Z} not numeric for gravity load: " + a.Description);
                }

                if (load.LoadType == LoadType.Point)
                {
                    var a = (LoadPoint) load;
                    if (!EmptyOrValidDouble(a.X))
                        ctx.AddError($"X: {a.X} not numeric for point load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Y))
                        ctx.AddError($"Y: {a.Y} not numeric for point load: " + a.Description);
                    if (!EmptyOrValidDouble(a.Z))
                        ctx.AddError($"Z: {a.Z} not numeric for point load: " + a.Description);
                    if (!EmptyOrValidDouble(a.XX))
                        ctx.AddError($"XX: {a.XX} not numeric for point load: " + a.Description);
                    if (!EmptyOrValidDouble(a.YY))
                        ctx.AddError($"YY: {a.YY} not numeric for point load: " + a.Description);
                    if (!EmptyOrValidDouble(a.ZZ))
                        ctx.AddError($"ZZ: {a.ZZ} not numeric for point load: " + a.Description);
                }
            }
        }

        protected bool IsInt(string v)
        {
            int _;
            return int.TryParse(v, out _);
        }

        protected bool IsDouble(string v)
        {
            double _;
            return double.TryParse(v, out _);
        }

        protected bool EmptyOrValidDouble(string v)
        {
            double _;
            bool isDouble = double.TryParse(v, out _);
            return isDouble || String.IsNullOrEmpty(v);
        }
    }
}