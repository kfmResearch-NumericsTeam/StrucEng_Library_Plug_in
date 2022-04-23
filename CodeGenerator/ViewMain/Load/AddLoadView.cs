using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class AddLoadView : DynamicLayout
    {
        private readonly AddLoadViewModel _vm;
        private DropDown _dropdownnLoads;
        private Button _btAddLoad;
        private DynamicLayout _container;

        public AddLoadView(AddLoadViewModel vm)
        {
            _vm = vm;
            BuildGui();
            BindGui();
        }

        private void BindGui()
        {
            _btAddLoad.Command = _vm.CommandAddLoad;
            _dropdownnLoads.ItemTextBinding = Binding.Property((LoadType t) => t.Name);
            _dropdownnLoads.DataStore = _vm.LoadsToAdd;
            _dropdownnLoads.Bind<LoadType>("SelectedValue", _vm, "SelectedLoadToAdd", DualBindingMode.TwoWay);
            _container.Bind<Control>("Content", _vm, "LoadViews", DualBindingMode.TwoWay);
        }

        protected void BuildGui()
        {
            AddRow(
                new GroupBox
                {
                    Text = "Define Load",
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
                                            new TableCell((_dropdownnLoads = new DropDown { }), true),
                                            new TableCell((_btAddLoad = new Button {Text = "Add"}))
                                        }
                                    },
                                }
                            },

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