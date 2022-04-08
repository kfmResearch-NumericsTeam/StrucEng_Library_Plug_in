using System;
using System.Text;
using CodeGenerator.Model;
using CodeGenerator.Utils;

namespace CodeGenerator
{
    public class PythonCodeGenerator
    {
        private readonly Workbench _model;

        public PythonCodeGenerator(Workbench model)
        {
            _model = model;
        }
        public string Generate()
        {
            // foreach (var l in _model.Layers)
            // {
            //     b.Append(String.Format("{0}: val: {1}, ", l.Name, l.Thickness));
            // }
            
            var gen = String.Format(PythonCodeSnippets.CODEGEN_HEADER, _model);
            return gen;   
        }
    }
}