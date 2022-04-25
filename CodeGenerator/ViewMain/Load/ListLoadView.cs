using CodeGenerator.Model;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class ListLoadView : DynamicLayout
    {
        private readonly ListLoadViewModel _vm;
        private DropDown _dbAddLoad;
        private DropDown _dbSelectLoad;
        private Button _btAddLoad;
        private DynamicLayout _container;
        private Button _btDeleteLoad;

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
            
            _dbAddLoad.BindDataContext (c => c.DataStore, (ListLoadViewModel m) => m.LoadNames);
            _dbAddLoad.SelectedKeyBinding.BindDataContext (Binding.Property((ListLoadViewModel m) => m.LoadName).EnumToString(), defaultContextValue: string.Empty);
            
            _dbSelectLoad.Bind<Load>("SelectedValue", _vm, "SelectedLoad", DualBindingMode.TwoWay);
            _dbSelectLoad.ItemTextBinding = Binding.Property((Load t) => t.GetType().GetName());
            _dbSelectLoad.DataStore = _vm.Loads;
            
            _container.Bind<Control>("Content", _vm, "LoadView", DualBindingMode.TwoWay);
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
                    new GroupBox
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
                                            new TableCell((_dbSelectLoad = new DropDown { }), true),
                                            new TableCell((_btDeleteLoad = new Button {Text = "Delete"}))
                                        }
                                    },
                                }
                            },
                        }
                    }
                });

            AddRow(
                new GroupBox
                {
                    Text = "Properties for Load",
                    Padding = new Padding(5),
                    Content = new DynamicLayout
                    {
                        Padding = new Padding(5),
                        // Padding = new Padding() {Top = 5, Left = 5, Bottom = 0, Right = 0},
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