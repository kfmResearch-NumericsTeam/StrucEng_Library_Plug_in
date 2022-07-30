using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Analysis;
using StrucEngLib.Load;
using StrucEngLib.LocalCoordinate;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib.Layer
{
    /// <summary>
    /// View to show layer information
    /// </summary>
    public class ListLayerView : DynamicLayout
    {
        private Button _btnMouseSelect;
        private Button _btnAddLayer;
        private Button _btnDeleteLayer;
        private ListBox _dropdownLayers;
        private TextBox _tbLayerToAdd;
        private GroupBox _gbPropertiesForLayer;
        private GroupBox _gbSelectLayer;
        private RadioButtonList _rdlElementSetSelection;

        private readonly LinFeMainViewModel _vm;
        private readonly ListLayerViewModel _vmListLayer;
        private readonly LayerDetailsViewModel _vmDetailView;

        public ListLayerView(LinFeMainViewModel vm)
        {
            _vm = vm;
            _vmListLayer = vm.ListLayerVm;
            _vmDetailView = vm.DetailLayerVm;


            BuildGui();
            BindGui();
        }

        protected void BindGui()
        {
            _btnAddLayer.Command = _vmListLayer.CommandOnAddLayer;
            _btnMouseSelect.Command = _vmListLayer.CommandOnMouseSelect;
            _btnDeleteLayer.Command = _vmListLayer.CommandOnDeleteLayer;

            _gbPropertiesForLayer.BindDataContext(
                c => c.Text,
                Binding.Property((ListLayerViewModel m) => m.SelectedLayer)
                    .CatchException(exception => true)
                    .Convert(l => l != null ? "Properties for " + l.GetName() : ""));

            _tbLayerToAdd.Bind<string>(nameof(_tbLayerToAdd.Text), _vmListLayer, nameof(_vmListLayer.LayerToAdd));
            _dropdownLayers.ItemTextBinding = Binding.Property((Model.Layer t) => t.ToString());
            _dropdownLayers.DataStore = _vmListLayer.Layers;
            _dropdownLayers.Bind<Model.Layer>(nameof(_dropdownLayers.SelectedValue), _vmListLayer,
                nameof(_vmListLayer.SelectedLayer));
            _rdlElementSetSelection.Bind<int>(nameof(_rdlElementSetSelection.SelectedIndex), _vmListLayer,
                nameof(_vmListLayer.LayerToAddType));
            _gbSelectLayer.Bind<bool>(nameof(_gbSelectLayer.Visible), _vmListLayer,
                nameof(_vmListLayer.SelectLayerViewVisible), DualBindingMode.TwoWay);
            _gbPropertiesForLayer.Bind<bool>(nameof(_gbPropertiesForLayer.Visible), _vmDetailView,
                nameof(_vmDetailView.LayerDetailViewVisible));
            _gbPropertiesForLayer.Bind<Control>(nameof(_gbPropertiesForLayer.Content), _vmDetailView,
                nameof(_vmDetailView.LayerDetailView));

            DataContext = _vmListLayer;
        }

        protected void BuildGui()
        {
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
                                            _btnAddLayer = new Button {Text = "Add Layer", Enabled = true})
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
                                            new TableCell((_dropdownLayers = new ListBox()
                                            {
                                            }), true),
                                            new TableCell(
                                                (TableLayout.AutoSized(_btnDeleteLayer = new Button {Text = "Delete"})))
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
                        Visible = false,
                        Content = _vmDetailView.LayerDetailView
                    }
                ));
            ScrollHelper.ScrollParent(_dropdownLayers);
        }
    }
}