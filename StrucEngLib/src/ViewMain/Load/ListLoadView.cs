using StrucEngLib.Views;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// Main view model to add/ remove loads
    /// </summary>
    public class ListLoadView : DynamicLayout
    {
        private readonly ListLoadViewModel _vm;
        private DropDown _dbAddLoad;
        private ListBox _dbSelectLoad;
        private Button _btAddLoad;
        private DynamicLayout _container;
        private Button _btDeleteLoad;
        private GroupBox _gbPropsForLoad;
        private GroupBox _gbSelectLoad;

        public ListLoadView(ListLoadViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BindGui()
        {
            _btAddLoad.Command = _vm.CommandAddLoad;
            _btDeleteLoad.Command = _vm.CommandDeleteLoad;

            _dbAddLoad.BindDataContext(c => c.DataStore, (ListLoadViewModel m) => m.LoadNames);
            _dbAddLoad.SelectedKeyBinding.BindDataContext(
                Binding.Property((ListLoadViewModel m) => m.LoadName).EnumToString(),
                defaultContextValue: string.Empty);

            _dbSelectLoad.Bind<Load>("SelectedValue", _vm, "SelectedLoad", DualBindingMode.TwoWay);
            _dbSelectLoad.ItemTextBinding = Binding.Property((Load t) => t.LoadType.GetName());
            _dbSelectLoad.DataStore = _vm.Loads;

            _container.Bind<Control>("Content", _vm, "LoadView", DualBindingMode.TwoWay);
            _gbPropsForLoad.Bind<bool>("Visible", _vm, "LoadViewVisible", DualBindingMode.TwoWay);
            _gbSelectLoad.Bind<bool>("Visible", _vm, "SelectLoadViewVisible", DualBindingMode.TwoWay);

            DataContext = _vm;
        }

        protected void BuildGui()
        {
            AddRow(
                new GroupBox
                {
                    Text = "Add Load",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new TableLayout
                            {
                                Spacing = new Size(5, 10),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((_dbAddLoad = new DropDown { }), true),
                                        }
                                    },
                                    new TableRow(
                                        TableLayout.AutoSized(
                                            _btAddLoad = new Button {Text = "Add", Enabled = true})
                                    )
                                }
                            }
                        }
                    }
                });
            AddRow(
                _gbSelectLoad = new GroupBox
                {
                    Text = "Select Load",
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
                                            new TableCell((_dbSelectLoad = new ListBox()
                                            {
                                            }), true),
                                            new TableCell(TableLayout.AutoSized((_btDeleteLoad = new Button {Text = "Delete"})))
                                        }
                                    },
                                }
                            },
                        }
                    }
                });
            AddRow(
                _gbPropsForLoad = new GroupBox
                {
                    Text = "Properties for Load",
                    Padding = new Padding(5),
                    Visible = false,
                    Content = new DynamicLayout

                    {
                        Padding = new Padding(5),
                        Spacing = new Size(5, 1),
                        Rows =
                        {
                            new TableLayout
                            {
                                Padding = new Padding(5),
                                Spacing = new Size(5, 5),
                                Rows =
                                {
                                    new TableRow
                                    {
                                        ScaleHeight = false, Cells =
                                        {
                                            new TableCell((_container = new DynamicLayout()), true)
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }
}