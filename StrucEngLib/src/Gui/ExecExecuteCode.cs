using System;
using System.IO;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Execute a python command</summary>
    public class ExecExecuteCode : CommandBase
    {
        private readonly MainViewModel _vm;
        private readonly string _code;

        // if = true, strucenglib will normalize non locked layers to match a reasonable font size
        private readonly bool _normalizeLayerFonts;

        public ExecExecuteCode(MainViewModel vm, string code, bool normalizeLayerFonts = true)
        {
            _vm = vm;
            _code = code;
            _normalizeLayerFonts = normalizeLayerFonts;
        }

        public override void Execute(object parameter)
        {
            try
            {
                OnGenerateModel(_code);
            }
            catch (Exception e)
            {
                _vm.ErrorVm.ShowException("Error during code execution", e);
            }
        }

        protected void OnGenerateModel(string source)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, source);
            StrucEngLibLog.Instance.WriteLine(
                "Executing a python command. The first run of this may take a while. Rhino will freeze until this operation is done");
            StrucEngLibLog.Instance.WriteLine("The temporary file being executed can be found at " + fileName);
            RhinoApp.RunScript("_-RunPythonScript " + fileName, true);

            if (_normalizeLayerFonts)
            {
                RhinoUtils.NormalizeTextHeights(RhinoDoc.ActiveDoc);
            }
        }
    }
}