using System;
using Eto.Drawing;
using Eto.Forms;

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
            AddRow(new GroupBox
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
        }

        private void BindGui()
        {
            DataContext = _vm;
            _dropdownLayers.DataContext = _vm;
            _dropdownLayers.BindDataContext(c => c.DataStore, (SmSettingViewModel vm) => vm.Properties);
            _dropdownLayers.ItemTextBinding =
                Binding.Property<SmAdditionalPropertyViewModel, string>(vm => vm.Model.Layer.GetName());

            _dropdownLayers.SelectedValueBinding.BindDataContext(
                Binding.Property<SmSettingViewModel, object>((SmSettingViewModel vm) => vm.SelectedProperty));

            _propLayoutHasData.Bind<bool>(nameof(_propLayoutHasData.Visible), _vm, nameof(_vm.HasLayers));
            _propLayoutNoData.Bind<bool>(nameof(_propLayoutNoData.Visible), _vm, nameof(_vm.HasNoLayers));
            _propLayoutHasData.Bind<object>(nameof(_propLayoutHasData.DataContext), _vm, nameof(_vm.SelectedProperty));


            _vm.ViewModelInitialized += (sender, args) =>
            {
                try
                {
                    // preselect first entry if possible
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