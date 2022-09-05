using System;
using System.Reflection;
using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for Additional Properties in Sandwich Model
    /// </summary>
    public class SmAdditionalPropertiesView : DynamicLayout
    {
        private readonly SmSettingViewModel _vm;
        private ListBox _dropdownLayers;
        private DynamicLayout _propLayout;
        private SmAdditionalPropertyView _propLayoutHasData;
        private DynamicLayout _propLayoutNoData;
        private GroupBox _gbProperties;

        public SmAdditionalPropertiesView(SmSettingViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BuildGui()
        {
            AddRow(new GroupBox
            {
                Text = "Select Layer",
                Padding = new Padding(5)
                {
                },
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
                                        }), true)
                                    }
                                },
                            }
                        },
                    }
                }
            });
            AddRow(_gbProperties = new GroupBox
            {
                Text = "Properties for Layer",
                Padding = new Padding(5),
                Content = _propLayout = new DynamicLayout
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5),
                }
            });
            _propLayoutNoData = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            _propLayoutNoData.Add(new Label() {Text = "No elements added to LinFe Model."});
            _propLayoutHasData = new SmAdditionalPropertyView();
            _propLayout.Add(_propLayoutHasData);
            _propLayout.Add(_propLayoutNoData);
                
            ImageViews();
            ScrollHelper.ScrollParent(_dropdownLayers);
        }
        
        private void ImageViews()
        {   
            var img01 = new System.Drawing.Bitmap(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "StrucEngLib.EmbeddedResources.sm_image01.png"));

            var img02 = new System.Drawing.Bitmap(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "StrucEngLib.EmbeddedResources.sm_image02.png"));

            var imageLeft = new ImageView()
            {
                Size = new Size(-1, 100),
                Image = Rhino.UI.EtoExtensions.ToEto(img01)
            };
            imageLeft.MouseDown += (sender, args) =>
            {
                var d = new ShowImageForm(img01)
                {
                    Owner = RhinoEtoApp.MainWindow
                };
                d.Show();
            };
            var imageRight = new ImageView()
            {
                Size = new Size(-1, 100),
                Image = Rhino.UI.EtoExtensions.ToEto(img02)
            };
            imageRight.MouseDown += (sender, args) =>
            {
                var d = new ShowImageForm(img02)
                {
                    Owner = RhinoEtoApp.MainWindow
                };
                d.Show();
            };
            
            AddRow(new GroupBox
            {
                Text = "Visualization",
                Padding = new Padding(5),
                Content = new DynamicLayout()
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 20),
                    Rows =
                    {
                        imageLeft,
                        imageRight
                    }
                }
            });
        }

        private void BindGui()
        {
            DataContext = _vm;
            
            _gbProperties.BindDataContext(
                c => c.Text,
                Binding.Property((SmSettingViewModel m) => m.SelectedProperty)
                    .CatchException(exception => true)
                    .Convert(l => l != null ? "Properties for Layer " + l.Model.Layer.GetName() : ""));


            _dropdownLayers.DataContext = _vm;
            _dropdownLayers.BindDataContext(c => c.DataStore, (SmSettingViewModel vm) => vm.Properties);
            _dropdownLayers.ItemTextBinding =
                Binding.Property<SmAdditionalPropertyViewModel, string>(vm => vm.Model.Layer.GetName());

            _dropdownLayers.SelectedValueBinding.BindDataContext(
                Binding.Property<SmSettingViewModel, object>((SmSettingViewModel vm) => vm.SelectedProperty));

            _propLayoutHasData.Bind<bool>(nameof(_propLayoutHasData.Visible), _vm, nameof(_vm.HasLayers));
            _propLayoutNoData.Bind<bool>(nameof(_propLayoutNoData.Visible), _vm, nameof(_vm.HasNoLayers));
            _propLayoutHasData.Bind<object>(nameof(_propLayoutHasData.DataContext), _vm, nameof(_vm.SelectedProperty));
            
            _dropdownLayers.SelectedValueChanged += (s, a) =>
            {
                _vm.RhinoSelectProperty();
            };

            _vm.ViewModelInitialized += (sender, args) =>
            {
                try
                {
                    // XXX: Preselect everything once to initialize data
                    for (var i = 0; i < _vm.Properties.Count; i++)
                    {
                        _dropdownLayers.SelectedIndex = i;
                    }
                    _dropdownLayers.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    // XXX: Ignore
                }
            };
        }
    }
}