using System;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;

namespace CodeGenerator
{
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C9")]
    public class CodeGenPanelView : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(CodeGenPanelView).GUID;

        private CodeGenPanelCtrl controller;
        
        public CodeGenPanelView()
        {
            controller = new CodeGenPanelCtrl();
            this.BuildGui();
        }

        private void BuildGui()
        {
            // selectLayerButton.Click += (sender, e) =>
            // {
            //     var doc = Rhino.RhinoDoc.ActiveDoc;
            //     var str = CodeGeneratorCommand.SelectLayer(doc);
            //     txtVal1.Text = str;
            // };
            var layout = new DynamicLayout();
            layout.Padding = new Padding(10, 10);
            layout.Spacing = new Size(0, 10);
            layout.AddRow(
                new GroupBox
                {
                    Text = "Generate Code",
                    Padding = new Padding(5),
                    Content = new TableLayout
                    {
                        Spacing = new Size(10, 10),
                        Rows =
                        {
                            new TableRow(
                                TableLayout.AutoSized(new Button {Text = "Inspect Python"}),
                                TableLayout.AutoSized(new Button {Text = "Generate Model"}))
                        }
                    }
                });
            layout.AddRow(
                new GroupBox
                {
                    Text = "Add Layer",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5), // padding around cells
                        Spacing = new Size(5, 5), // spacing between each cell
                        Rows =
                        {
                            new DynamicLayout
                            {
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new TextBox
                                    {
                                        PlaceholderText = "Type or Select Layer to add"
                                    }
                                }
                            },
                            new TableLayout
                            {
                                Spacing = new Size(10, 10),
                                Rows =
                                {
                                    new TableRow(
                                        TableLayout.AutoSized(new Button {Text = "Mouse Select"}),
                                        TableLayout.AutoSized(new Button {Text = "Add"})
                                    )
                                }
                            }
                        }
                    }
                });
            layout.AddRow(
                new GroupBox
                {
                    Text = "Select Layer",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new DynamicLayout
                            {
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new DropDown
                                    {
                                    }
                                }
                            },
                        }
                    }
                });
            layout.AddRow(
                new GroupBox
                {
                    Text = "Properties for Layer",
                    Padding = new Padding(5),
                    Content = new TableLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            // TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            // TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            // TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox())
                        }
                    }
                });
            Content = new Scrollable
            {
                Content = layout
            };
        }
        
        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
        {
        }

        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
        {
        }

        public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
        {
        }
    }
}