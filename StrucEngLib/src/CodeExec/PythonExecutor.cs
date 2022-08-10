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
    interface ICodeExecutor
    {
        void Execute(string pyCode, Action onDone = null);
    }
    
    /// <summary>Execute Python code within Rhino's Python Runtime</summary>
    public class PythonExecutor: ICodeExecutor
    {
        private const string FooterRes = "StrucEngLib.EmbeddedResources.exec_python_code_footer.py";
        private const string HeaderRes = "StrucEngLib.EmbeddedResources.exec_python_code_header.py";

        public void Execute(string pyCode, Action onDone = null)
        {
            var d = new FreezeDialog();
            d.Show();
            var code = AddCustomHandlers(pyCode);
            try
            {   
                new PythonExecutorMainThread().Execute(code, delegate
                {
                    d.Close();
                    onDone?.Invoke();
                });
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
                d.Close();
            }
        }
        
        public void ExecuteAsync(string pyCode, Action onDone = null)
        {
            var d = new FreezeDialog();
            d.Show();
            var code = AddCustomHandlers(pyCode);
            try
            {   
                new PythonExecutorBackground().Execute(code, delegate
                {
                    d.Close();
                    onDone?.Invoke();
                });
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
                d.Close();
            }
        }

        public string AddCustomHandlers(string snippet)
        {
            var headerTxt = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(HeaderRes) ??
                    throw new InvalidOperationException("Header resource not found"))
                .ReadToEnd();

            var footerTxt = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(FooterRes) ??
                    throw new InvalidOperationException("Footer resource not found"))
                .ReadToEnd();

            var snippetReader = new StringReader(snippet);
            var b = new StringBuilder();
            b.Append(headerTxt);
            b.Append(Environment.NewLine);

            string line = null;
            const string indent = "    ";
            while ((line = snippetReader.ReadLine()) != null)
            {
                b.Append(indent);
                b.Append(line);
                b.Append(Environment.NewLine);
            }

            b.Append(footerTxt);
            b.Append(Environment.NewLine);
            return b.ToString();
        }
    }
}