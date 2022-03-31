using System;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;

namespace CodeGenerator
{
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C9")]
    public class CodeGeneratorPanel : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(CodeGeneratorPanel).GUID;

        public CodeGeneratorPanel()
        {
            var sep0 = new Views.ViewSeparator {Text = "Elements"};
            var generateCodeButton = new Button {Text = "Mouse selectXs"};
            //generateCodeButton.Click += (sender, e) => OnGenerateCode();

            var txt1 = new Label {Text = "Layer"};
            var txtVal1 = new TextBox {Text = ""};

            var selectLayerButton = new Button {Text = "Mouse select"};
            selectLayerButton.Click += (sender, e) =>
            {
                var doc = Rhino.RhinoDoc.ActiveDoc;
                var str = CodeGeneratorCommand.SelectLayer(doc);
                txtVal1.Text = str;
            };

            // XsGroupBox

            // var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };

            //
            // var layout = new DynamicLayout();
            // //layout.Padding = new Padding(10);
            // //layout.Spacing = new Size(10, 10);
            // layout.BeginVertical(); 
            // layout.BeginHorizontal();
            // layout.Add(new Label { Text = "Layer" }, false);
            // layout.Add(txtVal1, true); // true == scale horizontally
            // layout.EndHorizontal();
            // layout.EndVertical();
            //
            // layout.BeginVertical();
            // layout.BeginHorizontal();
            //
            // layout.Add(selectLayerButton, false, false);
            // layout.Add(new Button { Text = "Add" }, false, false);
            //
            // layout.EndHorizontal();
            // layout.EndVertical();


            var layout = new DynamicLayout();
            layout.Padding = new Padding(10, 10);
            layout.Spacing = new Size(0, 10);
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
                            // TableLayout.Horizontal(
                            //     
                            //     null
                            //     )
                        }
                    }
                });


            // layout.AddRow(
            //     TableLayout.Horizontal(
            //         new Label {Text = "Layer"},
            //         new TableCell(new TextBox(), true)
            //         )
            // );
            layout.AddRow(
                new GroupBox
                {
                    Text = "Select Layer",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5), // padding around cells
                        Spacing = new Size(5, 1), // spacing between each cell
                        Rows =
                        {
                            new DynamicLayout
                            {
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    // new Label {Text = "Configure Layer: "},
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
                    Text = "Properties",
                    Padding = new Padding(5),
                    Content = new TableLayout
                    {
                        Padding = new Padding(5), // padding around cells
                        Spacing = new Size(5, 1), // spacing between each cell
                        Rows =
                        {
                            TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox()),
                            TableLayout.HorizontalScaled(new Label {Text = "Key"}, new TextBox())
                        }
                    }
                });
            
            // null,
            // new Rhino.UI.Controls.Divider(
            // ),
            // null,
            // new TableLayout
            // {
            //     Spacing = new Size(10, 10),
            //     Rows =
            //     {
            //         TableLayout.HorizontalScaled(
            //             new Label {Text = "Key"},
            //             new TextBox()
            //         ),
            //         TableLayout.HorizontalScaled(
            //             new Label {Text = "Key"},
            //             new TextBox()
            //         )
            //     }
            // }


            /*
            layout.BeginVertical(); // buttons section
            layout.BeginHorizontal();
            layout.Add(null, true); // add a blank space scaled horizontally to fill space
            layout.Add(new Button { Text = "Cancel" });
            layout.Add(new Button { Text = "Ok" });
            layout.EndHorizontal();
            layout.EndVertical();
            /*

            /*
            var layout = new DynamicLayout
            {
                Padding = new Padding(10),
                Spacing = new Size(10, 10),
                Rows = {
                new DynamicRow { Items = {
                    new Label { Text = "Layer" },
                    new TextBox()
                }
                },
                new Dynnam
                }
            }
            */


            var actions = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                Items = {null, generateCodeButton}
            };

            Content = new StackLayout
            {
                Padding = new Padding(10),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                {
                    new StackLayoutItem(sep0, HorizontalAlignment.Stretch),
                    new TableLayout
                    {
                        Padding = 10,
                        Rows =
                        {
                            new StackLayout
                            {
                                Orientation = Orientation.Vertical,
                                Spacing = 5,
                                Items = {txt1, txtVal1}
                            },

                            null, selectLayerButton
                        }
                    },
                    null,
                    actions
                }
            };

            Content = new Scrollable{
                Content = layout
            };
        }

        protected void OnSelectLayerClicked()
        {
            // Dialogs.ShowMessage("Hello Rhino!", "");
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