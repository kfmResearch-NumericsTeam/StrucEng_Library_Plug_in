using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Load
{
    using Load = StrucEngLib.Model.Load;

    /// <summary>
    /// Main view model to add/ remove loads
    /// </summary>
    public class ListLoadView : DynamicLayout
    {
        private readonly ListLoadViewModel _vm;
        private DropDown _dbAddLoad;
        private ListBox _lbSelectLoad;
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

            _gbPropsForLoad.BindDataContext(
                c => c.Text,
                Binding.Property((ListLoadViewModel m) => m.SelectedLoad)
                    .CatchException(exception => true)
                    .Convert(l => l != null ? "Properties for Load " + l.LoadType.GetName() : ""));


            _dbAddLoad.BindDataContext(c => c.DataStore, (ListLoadViewModel m) => m.LoadNames);
            _dbAddLoad.SelectedKeyBinding.BindDataContext(
                Binding.Property((ListLoadViewModel m) => m.LoadName).EnumToString(),
                defaultContextValue: string.Empty);

            _lbSelectLoad.Bind<Load>(nameof(_lbSelectLoad.SelectedValue), _vm, nameof(_vm.SelectedLoad));
            _lbSelectLoad.ItemTextBinding = Binding.Property((Load t) => t.LoadType.GetName());
            _lbSelectLoad.DataStore = _vm.Loads;

            _container.Bind<Control>(nameof(_container.Content), _vm, nameof(_vm.LoadView));
            _gbPropsForLoad.Bind<bool>(nameof(_gbPropsForLoad.Visible), _vm, nameof(_vm.LoadViewVisible));
            _gbSelectLoad.Bind<bool>(nameof(_gbSelectLoad.Visible), _vm, nameof(_vm.SelectLoadViewVisible));

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
                                            _btAddLoad = new Button {Text = "Add Load", Enabled = true})
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
                                            new TableCell((_lbSelectLoad = new ListBox()
                                            {
                                            }), true),
                                            new TableCell(
                                                TableLayout.AutoSized((_btDeleteLoad = new Button {Text = "Delete"})))
                                        }
                                    },
                                }
                            },
                        }
                    }
                });
            ScrollHelper.ScrollParent(_lbSelectLoad);
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