using System.Collections.Generic;
using CodeGenerator.Model;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class MainView : DynamicLayout
    {
        private Button _btnInspectPython;
        private Button _btnMouseSelect;
        private Button _btnAddLayer;
        private Button _btnDeleteLayer;
        private DropDown _dropdownLayers;
        private TextBox _tbLayerToAdd;
        private GroupBox _gbPropertiesForLayer;
        private RadioButtonList _rdlElementSetSelection;

        private MainViewModel _vm;

        public MainView(MainViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        protected void BindGui()
        {
            _btnAddLayer.Command = _vm.CommandOnAddLayer;
            _btnInspectPython.Command = _vm.CommandOnInspectCode;
            _btnMouseSelect.Command = _vm.CommandOnMouseSelect;
            _btnDeleteLayer.Command = _vm.CommandOnDeleteLayer;
            _tbLayerToAdd.Bind<string>("Text", _vm, "LayerToAdd", DualBindingMode.TwoWay);
            _dropdownLayers.ItemTextBinding = Binding.Property((Layer t) => t.ToString());
            _dropdownLayers.DataStore = _vm.Layers;
            _dropdownLayers.Bind<Layer>("SelectedValue", _vm, "SelectedLayer", DualBindingMode.TwoWay);
            _rdlElementSetSelection.Bind<int>("SelectedIndex", _vm, "LayerToAddType", DualBindingMode.TwoWay);
            _gbPropertiesForLayer.Bind<bool>("Visible", _vm, "PropertiesVisible", DualBindingMode.TwoWay);
            _gbPropertiesForLayer.Bind<Control>("Content", _vm, "PropertyContent", DualBindingMode.TwoWay);
        }

        protected void BuildGui()
        {
            Padding = new Padding(10, 10);
            Spacing = new Size(0, 10);
            AddRow(
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
                                    (_btnInspectPython = new Button {Text = "Inspect Model"}),
                                    true
                                )
                            )
                        }
                    }
                });
            AddRow(
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

                            new TableLayout()
                            {
                                Spacing = new Size(5, 5),
                                Padding = new Padding(0) {Top = 5, Bottom = 5},

                                Rows =
                                {
                                    new TableRow(_rdlElementSetSelection = new RadioButtonList()
                                    {
                                        Orientation = Orientation.Horizontal,
                                        DataStore = new[] {"Element      ", "Set   "},
                                        SelectedIndex = 0
                                    })
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
            AddRow(
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

            AddRow(
                (_gbPropertiesForLayer = new GroupBox
                    {
                        Text = "Properties for Layer",
                        Padding = new Padding(5),
                        Visible = false,
                        Content = _vm.PropertyContent
                    }
                ));
            
            // XXX: Last element gets scaled vertically
            AddRow(new Label {Text = ""});
        }
    }
}