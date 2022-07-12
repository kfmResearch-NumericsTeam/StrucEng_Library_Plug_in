using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Runtime;
using Rhino.UI.Controls;

namespace StrucEngLib
{
    public class Installer : Form
    {
        private string _abaqusHelp =
            "https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/wiki/Installation";

        private string _condaWebsite = "https://www.anaconda.com/products/distribution";

        private TextBox _tbAnaconda;
        private Button _btAbaqus;
        private Button _btInstallPython;
        private Button _btOpenCondaBin;
        private Button _btSelectConda;
        private Button _btBrowseConda;
        private Button _btTest;

        public Installer()
        {
            BuildGui();
            BindGui();
        }

        private string GetBatFile()
        {
            string fileName = Path.GetTempPath() + "install_" + Guid.NewGuid().ToString() + ".bat";
            var fileStream = File.Create(fileName);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "StrucEngLib.EmbeddedResources.install.bat");
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();
            return fileName;
        }

        private void BuildGui()
        {
            Maximizable = true;
            Minimizable = true;
            Padding = new Padding(5);
            Resizable = true;
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.Default;

            Title = "StrucEngLib Dependency Installer";

            var layout = new DynamicLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(10, 10),
            };
            layout.AddRow(new GroupBox()
            {
                Text = "Install or Specify Anaconda",
                Content = new DynamicLayout()
                {
                    Padding = new Padding(10),
                    Spacing = new Size(10, 15),
                    Rows =
                    {
                        TableLayout.HorizontalScaled(
                            new Label() {Text = "Anaconda Home Directory"},
                            (_tbAnaconda = new TextBox()
                            {
                                PlaceholderText = "e.g. C:\\Temp\\Miniconda3a3\\",
                            })
                        ),
                        TableLayout.HorizontalScaled(
                            (_btSelectConda = new Button() {Text = "Select Directory"}),
                            (_btBrowseConda = new Button() {Text = "Browse Website"})
                        ),
                    }
                }
            });

            layout.AddRow(new GroupBox()
            {
                Text = "Install Python Dependencies",
                Content = new DynamicLayout()
                {
                    Padding = new Padding(10),
                    Spacing = new Size(10, 15),
                    Rows =
                    {
                        new DynamicRow()
                        {
                            new Label() {Text = "Install Compas, Compas-FEA and SandwichModel"},
                        },
                        new DynamicRow() {
                        (_btInstallPython = new Button() {Text = "Install"})
                        },
                        TableLayout.HorizontalScaled(
                            (_btTest = new Button() {Text = "Test Import"}),
                            (_btOpenCondaBin = new Button() {Text = "Open condabin"})
                        ),
                        
                    }
                }
            });
            layout.AddRow(new GroupBox()
            {
                Text = "Install Abaqus",
                Content = new DynamicLayout()
                {
                    Padding = new Padding(10),
                    Spacing = new Size(10, 15),
                    Rows =
                    {
                        new DynamicRow()
                        {
                            new Label()
                            {
                                Text =
                                    "StrucEngLib needs Abaqus installed. Ensure Abaqus is available and restart Rhino."
                            },
                        },
                        new DynamicRow()
                        {
                            (_btAbaqus = new Button()
                            {
                                Text = "More Information",
                            })
                        }
                    }
                }
            });
            layout.AddRow(new Label());
            Content = layout;
        }

        private void BindGui()
        {
            _btAbaqus.Click += (sender, args) => { System.Diagnostics.Process.Start(_abaqusHelp); };
            _btBrowseConda.Click += (sender, args) => { System.Diagnostics.Process.Start(_condaWebsite); };
            _btInstallPython.Click += (sender, args) =>
            {
                string cmd = GetBatFile();
                if (String.IsNullOrWhiteSpace(_tbAnaconda.Text))
                {
                    RhinoApp.WriteLine("Anaconda home directory is null");
                    return;
                }

                var conda = _tbAnaconda.Text + "\\condabin\\conda.bat";
                if (!File.Exists(conda))
                {
                    RhinoApp.WriteLine("{0} does not exist!", conda);
                    return;
                }

                RhinoApp.WriteLine(cmd, conda);
                try
                {
                    System.Diagnostics.Process.Start(cmd, conda);
                }
                catch (Exception e)
                {
                    RhinoApp.WriteLine(e.Message);
                }
            };
            _btSelectConda.Click += (sender, args) =>
            {
                var dialog = new SelectFolderDialog();
                var result = dialog.ShowDialog(ParentWindow);
                if (result == DialogResult.Ok)
                {
                    RhinoApp.WriteLine("Result: {0}, Folder: {1}", result, dialog.Directory);
                    _tbAnaconda.Text = dialog.Directory;
                }
                else
                {
                    RhinoApp.WriteLine("Result: {0}", result);
                }
            };

            _btOpenCondaBin.Click += (sender, args) =>
            {
                if (String.IsNullOrWhiteSpace(_tbAnaconda.Text))
                {
                    RhinoApp.WriteLine("Anaconda home directory is null");
                    return;
                }

                var conda = _tbAnaconda.Text + "\\condabin\\";
                if (!Directory.Exists(conda))
                {
                    RhinoApp.WriteLine("{0} does not exist!", conda);
                    return;
                }
                try
                {
                    RhinoApp.WriteLine("cmd {0}", conda);
                    conda = conda.Replace(" ", "^ ");
                    System.Diagnostics.Process.Start("C:\\Windows\\System32\\cmd.exe", $"/K \"cd /d {conda} \"");
                }
                catch (Exception e)
                {
                    RhinoApp.WriteLine(e.Message);
                }
            };

            if (_btTest != null)
            {
                _btTest.Click += (sender, args) =>
                {
                    var cmd = $@"
import imp
imp.find_module('compas')
imp.find_module('compas_fea')
imp.find_module('Sandwichmodel')
print('success :)')";
                    new PythonExecutor().ExecuteCode(cmd);
                };
            }
        }
    }
}