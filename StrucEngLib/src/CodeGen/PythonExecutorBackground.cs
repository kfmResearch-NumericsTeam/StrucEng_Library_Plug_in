using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Runtime;
using Rhino.UI;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    public class PythonExecutorBackground : ICodeExecutor
    {
        public void Execute(string code, Action onDone = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += (sender, args) =>
            {
                StrucEngLibLog.Instance.WriteLine("execution completed");
                onDone?.Invoke();
            };

            RhinoApp.Wait();

            StrucEngLibLog.Instance.WriteLine("creating scripting context...");
            var ctx = Rhino.Runtime.PythonScript.Create();
            
            ctx.ScriptContextDoc = RhinoDoc.ActiveDoc;
            StrucEngLibLog.Instance.WriteLine("compiling code...");
            var compiled = ctx.Compile(code);
            ctx.Output = s =>
            {
                RhinoApp.InvokeOnUiThread(new Action(delegate
                {
                    StrucEngLibLog.Instance.WriteTaggedLine("exec", s);
                }));
                
            };
            
            object[] obs = {ctx, compiled, code};
            StrucEngLibLog.Instance.WriteLine("executing code...");
            worker.RunWorkerAsync(obs);
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            object[] obs = e.Argument as object[];
            PythonScript ctx = obs[0] as PythonScript;
            PythonCompiledCode compiled = obs[1] as PythonCompiledCode;
            string code = obs[2] as string;
            try
            {
                compiled.Execute(ctx);
            }
            catch (Exception ex)
            {
                RhinoApp.InvokeOnUiThread(new Action(delegate
                {
                    StringBuilder b = new StringBuilder("An exception occured while executing python code");
                    b.Append(ex.Message);
                    b.Append(Environment.NewLine);
                    b.Append(ex.StackTrace);
                    StrucEngLibPlugin.Instance.MainViewModel.ErrorVm.ShowMessage(b.ToString());
                }));
            }
        }
    }
}