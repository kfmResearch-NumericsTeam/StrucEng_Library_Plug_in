using Eto.Forms;

namespace CodeGenerator.mvvc
{
    public class MyView: DynamicLayout
    {
        public Button BtTest { get; private set; }
        public TextBox TbTest { get; private set; }

        private MyViewModel _vm;
        

        public MyView(MyViewModel vm)
        {
            _vm = vm;
            
            BtTest = new Button();
            BtTest.Text = "Test";
            TbTest = new TextBox();
            Add(BtTest);
            Add(TbTest);

            DoBindings();
        }

        private void DoBindings()
        {
            TbTest.Bind<string>("Text", _vm, "Value", DualBindingMode.TwoWay);
            BtTest.Command = _vm.TestCommand;
        }
        
    }
}