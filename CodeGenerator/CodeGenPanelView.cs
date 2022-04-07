using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeGenerator.ui_model;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using Rhino.UI.CodeEditor;

namespace CodeGenerator
{
    [System.Runtime.InteropServices.Guid("0E7780CA-F004-4AE7-B918-19E68BF7C7C9")]
    public class CodeGenPanelView : Panel, IPanel
    {
        public static System.Guid PanelId => typeof(CodeGenPanelView).GUID;

        public static CodeGenPanelView Instance { get; private set; }

        protected CodeGenPanelCtrl Controller;

        protected CodeGenPanelModel Model;

        // Set to True once ctrl/ model are wired up
        // XXX: We need this because rhino calls the constructor for us
        public bool Initialized { get; set; }

        private Button _btnGenerateModel;
        private Button _btnInspectPython;
        private Button _btnMouseSelect;
        private Button _btnAddLayer;
        private Button _btnDeleteLayer;
        private DropDown _dropdownLayers;
        private TextBox _tbLayerToAdd;
        private GroupBox _gbPropertiesForLayer;

        public CodeGenPanelView()
        {
            Rhino.RhinoApp.WriteLine("Creating view");
            Instance = this;
            BuildGui();
        }

        public void RegisterModel(CodeGenPanelModel model)
        {
            this.Model = model;
            UpdateView();
        }

        public void RegisterController(CodeGenPanelCtrl ctrl)
        {
            Controller = ctrl;
            _btnGenerateModel.Click += (sender, e) => ctrl.OnGenerateModel();
            _btnInspectPython.Click += (sender, e) => ctrl.OnInspectPython();
            _btnAddLayer.Click += (sender, e) => ctrl.OnAddLayer(_tbLayerToAdd.Text);
            _btnMouseSelect.Click += (sender, e) => ctrl.OnMouseSelectLayer();
            _btnDeleteLayer.Click += (sender, e) => ctrl.OnLayerDelete();
            _dropdownLayers.SelectedIndexChanged += (sender, e) =>
            {
                ctrl.OnSelectLayerInDropdown(_dropdownLayers.SelectedIndex);
            };

            // XXX: We write here "enabled" logic in view to reduce boilerplate code
            _tbLayerToAdd.TextBinding.Bind(() => _tbLayerToAdd.Text, val => { _btnAddLayer.Enabled = val != ""; });
        }

        public void UpdateView()
        {
            _tbLayerToAdd.Text = Model.LayerToAdd;
            List<string> dbLayers = new List<string>();

            {
                int i = 0;
                int selection = 0;
                foreach (var l in Model.Layers)
                {
                    if (l == Model.CurrentLayer)
                    {
                        selection = i;
                    }

                    dbLayers.Add(l.Name);
                    i++;
                }

                _dropdownLayers.SelectedIndex = selection;
                _dropdownLayers.DataStore = dbLayers;
            }

            _gbPropertiesForLayer.Visible = Model.CurrentLayer != null;

            _btnInspectPython.Enabled = Model.Layers.Count > 0;
            _btnGenerateModel.Enabled = Model.Layers.Count > 0;
            _btnDeleteLayer.Enabled = Model.Layers.Count > 0;

            if (Model.CurrentLayer != null)
            {
                _gbPropertiesForLayer.Text = "Properties for Layer " + Model.CurrentLayer.Name;
                _gbPropertiesForLayer.Visible = true;
                UpdatePropertyLayer(Model.CurrentLayer);
            }
        }

        private void BuildGui()
        {
            DataContext = Model;

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
                                new TableCell(
                                    (_btnInspectPython = new Button {Text = "Inspect Code"}),
                                    true
                                ),
                                new TableCell(
                                    (_btnGenerateModel = new Button {Text = "Generate Model"}),
                                    true
                                )
                            )
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
                        Padding = new Padding(5),
                        Spacing = new Size(5, 10),
                        Rows =
                        {
                            new TableLayout()
                            {
                                Spacing = new Size(5, 5),

                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell(
                                                (_tbLayerToAdd = new TextBox
                                                    {PlaceholderText = "Type or Select Layer to add",}
                                                ),
                                                true),
                                            new TableCell((_btnMouseSelect = new Button {Text = "Select..."}))
                                        }
                                    },
                                }
                            },
                            new TableLayout
                            {
                                Spacing = new Size(10, 10),
                                Rows =
                                {
                                    new TableRow(
                                        TableLayout.AutoSized(
                                            _btnAddLayer = new Button {Text = "Add", Enabled = false})
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
                            new TableLayout
                            {
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((_dropdownLayers = new DropDown { }), true),
                                            new TableCell((_btnDeleteLayer = new Button {Text = "Delete"}))
                                        }
                                    },
                                }
                            },
                        }
                    }
                });

            layout.AddRow(
                (_gbPropertiesForLayer = new GroupBox
                    {
                        Text = "Properties for Layer",
                        Padding = new Padding(5),
                        Visible = false,
                    }
                ));

            // XXX: Last element gets scaled vertically
            layout.AddRow(new Label {Text = ""});
            Content = new Scrollable {Content = layout};
        }

        private void UpdatePropertyLayer(Layer l)
        {
            var layout = new TableLayout
            {
                Padding = new Padding(0),
                Spacing = new Size(5, 1),
            };

            _gbPropertiesForLayer.Content = layout;
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