using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Rhino;
using Rhino.UI;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Generate and Execute Code</summary>
    public class ExecGenerateCode: AsyncCommandBase
    {
        private readonly MainViewModel _vm;

        public ExecGenerateCode(MainViewModel vm)
        {
            _vm = vm;
        }
        public override Task ExecuteAsync(object parameter)
        {
            Workbench model =  _vm.BuildModel();
            ModelValidator validate = new ModelValidator();
            var valMsgs = validate.ValidateModel(model);
            if (valMsgs.Count != 0)
            {
                _vm.ErrorVm.ShowMessages(valMsgs);
                return null;
            }
            PythonCodeGenerator codeGen = new PythonCodeGenerator(model);
            var sourceCode = codeGen.Generate();
            
            var dialog = new InspectPythonDialog(sourceCode);
            var dialogRc = dialog.ShowSemiModal(RhinoDoc.ActiveDoc, RhinoEtoApp.MainWindow);
            if (dialogRc == Eto.Forms.DialogResult.Ok)
            {
                sourceCode = dialog.Source;

                if (dialog.State == InspectPythonDialog.STATE_EXEC)
                {
                    OnGenerateModel(sourceCode);
                } 
            }

            return null;
        }
        
        protected void OnGenerateModel(string source)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".py";
            File.WriteAllText(fileName, source);
            Rhino.RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }
    }
}