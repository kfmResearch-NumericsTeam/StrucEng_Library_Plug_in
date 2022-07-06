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
                FileName = "C:\\tmp"
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

            AddSandwichData(b);
            var gen = new PythonCodeGenerator();
            AssertValidPythonCode(gen.GenerateSmmCode(b));
        }

        private void AddSandwichData(Workbench b)
        {
            var prop = new SandwichProperty()
            {
                DStrichBot = "123",
                DStrichTop = "123",
                FcK = "123",
                FcThetaGradKern = "123",
                FsD = "123",
                AlphaBot = "123",
                BetaBot = "123",
                AlphaTop = "123",
                BetaTop = "123",
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

            var set = new SmAnalysisSetting()
            {
                Include = true,
                Step = step,
                AxesScale = "1",
                DruckzonenIteration = "1",
                MindestBewehrung = "1",
                Code = "1",
                Schubnachweis = "1",
                AsXiBot = true,
                AsXiTop = true,
                AsEtaBot = true,
                AsEtaTop = true,
                AsZ = true,
                CCBot = true,
                CCTop = true,
                KBot = true,
                KTop = true,
                TBot = true,
                TTop = true,
                PsiBot = true,
                PsiTop = true,
                FallBot = true,
                FallTop = true,
                MCcBot = true,
                MCcTop = true,
                MShearC = true,
                MCTotal = true,
            };


            prop.Layer = b.Layers[0];

            b.SandwichModel = new SandwichModel()
            {
            };
            b.SandwichModel.AdditionalProperties.Add(prop);
            b.SandwichModel.AnalysisSettings.Add(set);
        }
    }
}