using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Rhino;
using Rhino.UI;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>Execute Python code within Rhino's Python Runtime. Curate code with error handling</summary>
    public class PythonExecutor
    {
        private const string FooterRes = "StrucEngLib.EmbeddedResources.exec_python_code_footer.py";
        private const string HeaderRes = "StrucEngLib.EmbeddedResources.exec_python_code_header.py";

        private void Exec(string code)
        {
            string fileName = Path.GetTempPath() + "strucenglib_" + Guid.NewGuid() + ".py";

            File.WriteAllText(fileName, code);
            StrucEngLibLog.Instance.WriteLine("Executing file: " + fileName + "\n");
            RhinoApp.Wait();
            RhinoApp.RunScript("_-RunPythonScript " + fileName, true);
        }

        public void ExecuteCode(string snippet)
        {
            var d = new FreezeDialog();
            d.Show();
            var code = AddCustomHandlers(snippet);
            try
            {
                Exec(code);
            }
            catch (Exception e)
            {
                StrucEngLibLog.Instance.WriteLine(e.Message);
            }
            finally
            {
                d.Close();
            }
        }

        private string AddCustomHandlers(string snippet)
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