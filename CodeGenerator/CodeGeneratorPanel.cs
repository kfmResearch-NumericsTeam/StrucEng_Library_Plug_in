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
            var sep0 = new Views.ViewSeparator { Text = "Elements" };
            var generateCodeButton = new Button { Text = "Generate Code" };
            //generateCodeButton.Click += (sender, e) => OnGenerateCode();

            var txt1 = new Label { Text = "Layer" };
            var txtVal1 = new TextBox { Text = "" };

            var selectLayerButton = new Button { Text = "Select Layer" };
            selectLayerButton.Click += (sender, e) => {
                var doc = Rhino.RhinoDoc.ActiveDoc;
                var str = CodeGeneratorCommand.SelectLayer(doc);
                txtVal1.Text = str;
                };

            var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };
            
            var actions = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                Items = { null, generateCodeButton}
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
                        Rows = {
                            new StackLayout
                            {
                                Orientation = Orientation.Vertical,
                                Spacing = 5,
                                Items = {  txt1 , txtVal1}
                            },

                        null, selectLayerButton 
                        }
                    },
                    null,
                    actions
                }
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



