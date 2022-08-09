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
    /// <summary>Tests for PythonCodeExector</summary>
    [TestFixture]
    public class PythonExecutorTest
    {
        
        public CompiledCode CompilePython(string c)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource scriptSource = engine.CreateScriptSourceFromString(c);
            CompiledCode code = scriptSource.Compile();
            return code;
        }
    }
}