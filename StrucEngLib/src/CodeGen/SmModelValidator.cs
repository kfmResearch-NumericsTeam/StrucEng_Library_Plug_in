using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Runtime.InteropWrappers;
using StrucEngLib;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>Validation for Sandwich Model</summary>
    public class SmModelValidator
    {
        public ErrorMessageContext ValidateModel(Workbench model)
        {
            var ctx = new ErrorMessageContext()
            {
                ContextDescription = "Sandwich Model"
            };

            if (model.SandwichModel == null)
            {
                ctx.AddError("No Sandwich Model defined");
            }
            else
            {
                var sm = model.SandwichModel;
                if (sm.AdditionalProperties == null || sm.AdditionalProperties.Count == 0)
                {
                    ctx.AddInfo("No additional properties defined");
                }
                else
                {
                    foreach (var ap in sm.AdditionalProperties)
                    {
                        if (ap.Layer == null)
                        {
                            ctx.AddError("No layer defined for additional property");
                        }
                        else
                        {
                            var info = ap.Layer.GetName();
                            MustBeDefined(ap.AlphaBot, "AlphaBot", ctx, info);
                            MustBeDefined(ap.AlphaTop, "AlphaTop", ctx, info);
                            MustBeDefined(ap.BetaBot, "BetaBot", ctx, info);
                            MustBeDefined(ap.BetaTop, "BetaTop", ctx, info);
                            MustBeDefined(ap.FcK, "FcK", ctx, info);
                            MustBeDefined(ap.FsD, "FsD", ctx, info);
                            MustBeDefined(ap.DStrichBot, "DStrichBot", ctx, info);
                            MustBeDefined(ap.DStrichTop, "DStrichTop", ctx, info);
                            MustBeDefined(ap.FcThetaGradKern, "FcThetaGradKern", ctx, info);
                        }
                    }
                }

                bool hasStep = false;
                sm.AnalysisSettings?.ForEach(s =>
                {
                    hasStep = hasStep || s.Include;
                    if (s.Step == null)
                    {
                        ctx.AddError("Invalid step (null) in analysis settings");
                    }
                    else if (String.IsNullOrWhiteSpace(s.Step.Order) && s.Include)
                    {
                        ctx.AddError("Invalid Step assigned to Sandwich Model");
                    }
                    
                    
                    // dependency on linfe
                    if (s.Include)
                    {
                        var step = s.Step;
                        if (step.Setting != null)
                        {
                            var setting = step.Setting;
                            // Sandwich model requires analysis output sf1-5 and sm
                            if (!setting.ShellForces)
                            {
                                ctx.AddWarning($"Sandwichmodel requires analysis output 'Shell forces' in step {step.Order} of LinFE model." +
                                               $" Auto corrected.");
                                setting.ShellForces = true;
                            }
                            if (!setting.SectionMoments)
                            {
                                ctx.AddWarning($"Sandwichmodel requires analysis output 'Section moments' or 'Shell moments' in step {step.Order} of LinFE model." +
                                               $" Auto corrected");
                                setting.SectionMoments = true;
                            }
                        }
                    }
                    
                });
                if (!hasStep)
                {
                    ctx.AddWarning("No Step included in Output of Sandwich Model");
                }
            }

            return ctx;
        }

        protected void MustBeDefined(string value, string name, ErrorMessageContext ctx, string messageInfo = "")
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                ctx.AddWarning($"{name} is empty {messageInfo}");
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