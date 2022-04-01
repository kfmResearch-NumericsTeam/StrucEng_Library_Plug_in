using System;
using System.Text;

namespace CodeGenerator
{
    public class PythonCodeGenerator
    {
        private readonly CodeGenPanelModel _model;

        public PythonCodeGenerator(CodeGenPanelModel model)
        {
            _model = model;
        }

        public string Generate()
        {
            var b = new StringBuilder();
            foreach (var l in _model.Layers)
            {
                b.Append(String.Format("{0}: val: {1}, ", l.Name, l.KeyVal1));
            }
            var gen = String.Format(PythonCodeSnippets.CODEGEN_HEADER, b.ToString());
            return gen;
        }
    }
}