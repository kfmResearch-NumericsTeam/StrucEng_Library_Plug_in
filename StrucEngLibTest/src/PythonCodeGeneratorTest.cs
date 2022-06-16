using System;
using IronPython.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.Scripting.Hosting;
using NUnit.Framework;
using StrucEngLib;
using StrucEngLib.Model;

namespace StrucEngLibTest;

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
        Console.WriteLine(res);
        Assert.IsNotEmpty(res);
        Assert.IsNotNull(res);

        var code = CompilePython(res);
        Console.WriteLine(code.ToString());
        Assert.NotNull(code.ToString());
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
        Console.WriteLine(res);
        Assert.IsNotEmpty(res);
        Assert.IsNotNull(res);

        var code = CompilePython(res);
        Console.WriteLine(code.ToString());
        Assert.NotNull(code.ToString());
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
        Console.WriteLine(res);
        Assert.IsNotEmpty(res);
        Assert.IsNotNull(res);

        var code = CompilePython(res);
        Console.WriteLine(code.ToString());
        Assert.NotNull(code.ToString());
    }
}