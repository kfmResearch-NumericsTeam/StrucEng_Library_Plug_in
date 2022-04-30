using System.Collections.Generic;
using System.Drawing;
using CodeGenerator.Model;
using CodeGenerator.Step;
using CodeGenerator.Step;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;
using Font = Eto.Drawing.Font;
using FontStyle = Eto.Drawing.FontStyle;
using Size = Eto.Drawing.Size;

namespace CodeGenerator
{
    /// <summary>
    /// Main view to show layer information
    /// </summary>
    public class ListLayerView : DynamicLayout
    {
        private Button _btnInspectPython;
        private Button _btnMouseSelect;
        private Button _btnAddLayer;
        private Button _btnDeleteLayer;
        private ListBox _dropdownLayers;
        private TextBox _tbLayerToAdd;
        private GroupBox _gbPropertiesForLayer;
        private GroupBox _gbSelectLayer;
        private RadioButtonList _rdlElementSetSelection;

        private readonly ListLayerViewModel _vmListLayer;
        private readonly LayerDetailsViewModel _vmDetailView;
        private readonly ListLoadViewModel _vmListLoad;
        private readonly ListStepViewModel _vmListStep;

        public ListLayerView(MainViewModel vm)
        {
            _vmListLayer = vm.ListLayerVm;
            _vmDetailView = vm.DetailLayerVm;
            _vmListStep = vm.ListStepVm;
            _vmListLoad = vm.ListLoadVm;
            
            BuildGui();
            BindGui();
        }

        protected void BindGui()
        {
            _btnAddLayer.Command = _vmListLayer.CommandOnAddLayer;
            _btnInspectPython.Command = _vmListLayer.CommandOnInspectCode;
            _btnMouseSelect.Command = _vmListLayer.CommandOnMouseSelect;
            _btnDeleteLayer.Command = _vmListLayer.CommandOnDeleteLayer;
            
            _tbLayerToAdd.Bind<string>("Text", _vmListLayer, "LayerToAdd", DualBindingMode.TwoWay);
            _dropdownLayers.ItemTextBinding = Binding.Property((Layer t) => t.ToString());
            _dropdownLayers.DataStore = _vmListLayer.Layers;
            _dropdownLayers.Bind<Layer>("SelectedValue", _vmListLayer, "SelectedLayer", DualBindingMode.TwoWay);
            _rdlElementSetSelection.Bind<int>("SelectedIndex", _vmListLayer, "LayerToAddType", DualBindingMode.TwoWay);
            _gbSelectLayer.Bind<bool>("Visible", _vmListLayer, "SelectLayerViewVisible", DualBindingMode.TwoWay);
            
            _gbPropertiesForLayer.Bind<bool>("Visible", _vmDetailView, "LayerDetailViewVisible", DualBindingMode.TwoWay);
            _gbPropertiesForLayer.Bind<Control>("Content", _vmDetailView, "LayerDetailView", DualBindingMode.TwoWay);
        }

        protected Control GenerateTitle(string text)
        {
            var s = new Label().Font.Size;
            return new ViewSeparator()
            {
                Text = text,
                Label =
                {
                    Font = new Font(FontFamilies.Sans, s, FontStyle.Bold)  
                }
            };
        }

        protected void BuildGui()
        {
            Padding = new Padding(10, 10);
            Spacing = new Size(0, 10);

            AddRow(GenerateTitle("Settings"));
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
            
            AddRow(GenerateTitle("Layers"));
            AddRow(
                new GroupBox
                {
                    Text = "Add Layer",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
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
                                                    {
                                                        PlaceholderText = "Type or Select Layer to add",
                                                        AutoSelectMode = AutoSelectMode.OnFocus
                                                    }
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
                                            _btnAddLayer = new Button {Text = "Add", Enabled = true})
                                    )
                                }
                            }
                        }
                    }
                });
            AddRow(
                _gbSelectLayer = new GroupBox
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
                                            new TableCell((_dropdownLayers = new ListBox() { }), true),
                                            new TableCell((TableLayout.AutoSized(_btnDeleteLayer = new Button {Text = "Delete"})))
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
                        Content = _vmDetailView.LayerDetailView
                    }
                ));

            AddRow(GenerateTitle("Loads"));
            AddRow(new ListLoadView(_vmListLoad));
            
            AddRow(GenerateTitle("Steps"));
            AddRow(new StepView(_vmListStep));
            
            // XXX: Last element gets scaled vertically
            AddRow(new Label {Text = ""});
        }
    }
}