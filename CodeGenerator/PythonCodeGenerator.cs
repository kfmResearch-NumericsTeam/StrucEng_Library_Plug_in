using System;

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
            var gen = String.Format(PythonCodeSnippets.CODEGEN_HEADER, _model.ToString());
            return gen;
        }
    }
}