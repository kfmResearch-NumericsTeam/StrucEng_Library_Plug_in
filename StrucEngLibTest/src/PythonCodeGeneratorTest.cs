using System;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.Scripting.Hosting;
using NUnit.Framework;
using StrucEngLib;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;

namespace StrucEngLibTest
{

    /// <summary></summary>
    [TestFixture]
    public class PythonCodeGeneratorTest
    {
        public CompiledCode CompilePython(string c)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource scriptSource = engine.CreateScriptSourceFromString(c);
            CompiledCode code = scriptSource.Compile();
            return code;
        }

        [Test]
        public void TestGenerateLinFeCode1()
        {
            var b = WorkbenchUtils.CreateSampleWorkBench();
            var gen = new PythonCodeGenerator();
            var res = gen.GenerateLinFeCode(b);
            Console.WriteLine(res);
            Assert.IsNotEmpty(res);
            Assert.IsNotNull(res);

            var code = CompilePython(res);
            Console.WriteLine(code.ToString());
            Assert.NotNull(code.ToString());
        }

        [Test]
        public void TestEmptyWorkbench()
        {
            var b = new Workbench();
            var gen = new PythonCodeGenerator();
            var res = gen.GenerateLinFeCode(b);
            AssertValidPythonCode(res);
        }

        [Test]
        public void TestSingleElement()
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
            var gen = new PythonCodeGenerator();
            var res = gen.GenerateLinFeCode(b);
            AssertValidPythonCode(res);
        }

        [Test]
        public void TestSingleSet()
        {
            Workbench b = new Workbench();
            b.Layers.Add(new Set()
            {
                Name = "Set1",
                SetDisplacementType = SetDisplacementType.PINNED,
            });
            var gen = new PythonCodeGenerator();
            var res = gen.GenerateLinFeCode(b);
            AssertValidPythonCode(res);
        }

        private void AssertValidPythonCode(string res)
        {
            Console.WriteLine(res);
            Assert.IsNotEmpty(res);
            Assert.IsNotNull(res);

            var code = CompilePython(res);
            Console.WriteLine(code.ToString());
            Assert.NotNull(code.ToString());
        }

        [Test]
        public void TestSandwichModel()
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
            b.SandwichModel = new SandwichModel()
            {
                StepName = "1"
            };
            var gen = new PythonCodeGenerator();
            AssertValidPythonCode(gen.GenerateSmmCode(b));
        }

        [Test]
        public void TestSwWithProperties()
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

            b.SandwichModel = new SandwichModel()
            {
                StepName = "1"
            };
            var step = new Step()
            {
                Order = "1",
                Setting = new AnalysisSetting()
                {
                    Include = true,
                    Cf = true,
                    StepId = "1"
                }
            };
            b.Steps.Add(step);
            b.SandwichModel.Code = "sia";
            b.SandwichModel.StepName = "1";
            b.SandwichModel.AsEtaBot = true;
            b.SandwichModel.AsXiBot = true;
            b.SandwichModel.AsXiTop = true;
            b.SandwichModel.AsEtaBot = true;
            b.SandwichModel.AsEtaTop = true;
            b.SandwichModel.AsZ = true;
            b.SandwichModel.CCBot = true;
            b.SandwichModel.CCTop = true;
            b.SandwichModel.KBot = true;
            b.SandwichModel.KTop = true;
            b.SandwichModel.TBot = true;
            b.SandwichModel.TTop = true;
            b.SandwichModel.PsiBot = true;
            b.SandwichModel.PsiTop = true;
            b.SandwichModel.FallBot = true;
            b.SandwichModel.FallTop = true;
            b.SandwichModel.MCcBot = true;
            b.SandwichModel.MCcTop = true;
            b.SandwichModel.MShearC = true;
            b.SandwichModel.MCTotal = true;

            b.SandwichModel.AdditionalProperties.Add(new SandwichProperty()
            {
                Layer = b.Layers[0],
                AlphaBot = "123"
            });
            var gen = new PythonCodeGenerator();
            AssertValidPythonCode(gen.GenerateSmmCode(b));
        }
    }
}