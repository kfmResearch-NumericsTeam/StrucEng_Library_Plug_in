using System.Collections.Generic;
using System.Linq;
using StrucEngLib.Model;

namespace StrucEngLibTest;

/// <summary></summary>
public class WorkbenchUtils
{
    public static Workbench CreateSampleWorkBench()
    {
        Workbench b = new Workbench();
        b.Layers.Add(new Element()
        {
            ElementMaterialElastic = new ElementMaterialElastic()
            {
                E = "1.0",
                P = "1.0",
                V = "1.0"
            },
            ElementShellSection = new ElementShellSection()
            {
                Thickness = "100",
            },
            Name = "TestElement1",
            LoadConstraint = new ElementLoadConstraint()
            {
                Ex0 = 0,
                Ex1 = 1,
                Ex2 = 2,
                Ey0 = 3,
                Ey1 = 4,
                Ey2 = 5,
                Ez0 = 6,
                Ez1 = 7,
                Ez2 = 8,
                ElementNumber = 100
            }
        });

        b.Layers.Add(new Element()
        {
            ElementMaterialElastic = new ElementMaterialElastic()
            {
                E = "2.0",
                P = "3.0",
                V = "4.0"
            },
            ElementShellSection = new ElementShellSection()
            {
                Thickness = "200",
            },
            Name = "TestElement2",
            LoadConstraint = new ElementLoadConstraint()
            {
                Ex0 = 0,
                Ex1 = 1,
                Ex2 = 2,
                Ey0 = 3,
                Ey1 = 4,
                Ey2 = 5,
                Ez0 = 6,
                Ez1 = 7,
                Ez2 = 8,
                ElementNumber = 500
            }
        });

        b.Layers.Add(new Element()
        {
            ElementMaterialElastic = new ElementMaterialElastic()
            {
                E = "2.0",
                P = "3.0",
                V = "4.0"
            },
            ElementShellSection = new ElementShellSection()
            {
                Thickness = "400",
            },
            Name = "TestElement3",
            LoadConstraint = new ElementLoadConstraint()
            {
                Ex0 = 0,
                Ex1 = 1,
                Ex2 = 2,
                Ey0 = 3,
                Ey1 = 4,
                Ey2 = 5,
                Ez0 = 6,
                Ez1 = 7,
                Ez2 = 8,
                ElementNumber = 600
            }
        });

        b.Layers.Add(new Set()
        {
            Name = "Set1",
            SetDisplacementType = SetDisplacementType.PINNED,
        });

        b.Layers.Add(new Set()
        {
            Name = "Set2",
            SetDisplacementType = SetDisplacementType.GENERAL,
            SetGeneralDisplacement = new SetGeneralDisplacement()
            {
                Rotx = "1",
                Roty = "2",
                Rotz = "3",
                Ux = "4",
                Uy = "5",
                Uz = "6"
            }
        });

        var area = new LoadArea()
        {
            Layers = new List<Layer>()
        };
        area.Layers.Add(b.Layers[0]);
        area.Layers.Add(b.Layers[2]);
        b.Loads.Add(area);

        var gravity = new LoadGravity()
        {
            Layers = new List<Layer>()
        };
        gravity.Layers.Add(b.Layers[0]);
        gravity.Layers.Add(b.Layers[1]);
        b.Loads.Add(gravity);

        var point = new LoadPoint()
        {
            Layers = new List<Layer>()
        };
        point.Layers.Add(b.Layers[0]);
        b.Loads.Add(point);

        b.Steps.Add(new Step()
        {
            Order = "1",
            Setting = new AnalysisSetting()
            {
                Include = true,
                StepId = "1",
                Cf = true,
            },
            Entries =
            {
                new StepEntry()
                {
                    Type = StepType.Load,
                    Value = b.Loads[0],
                }
            }
        });

        b.Steps.Add(new Step()
        {
            Order = "2",
            Setting = new AnalysisSetting()
            {
                Include = true,
                StepId = "2",
                U = true,
            },
            Entries =
            {
                new StepEntry()
                {
                    Type = StepType.Set,
                    Value = b.Layers.Where(l => l.LayerType == LayerType.SET).ToList()[0],
                },
                new StepEntry()
                {
                    Type = StepType.Set,
                    Value = b.Layers.Where(l => l.LayerType == LayerType.SET).ToList()[1],
                },
            }
        });

        b.Steps.Add(new Step()
        {
            Order = "3",
            Setting = new AnalysisSetting()
            {
                Include = true,
                StepId = "3",
                Cm = true,
            },
            Entries =
            {
                new StepEntry()
                {
                    Type = StepType.Load,
                    Value = b.Loads[0],
                },
                new StepEntry()
                {
                    Type = StepType.Set,
                    Value = b.Layers.Where(l => l.LayerType == LayerType.SET).ToList()[0],
                },
            }
        });
        return b;
    }
}