using CodeGenerator.ui_model;
using Eto.Forms;

namespace CodeGenerator

{
    public class PropertyCtrl
    {
        public PropertyView View { get; private set; }
        public SimplePropertyModel Model { get; private set; }

        public PropertyCtrl(SimplePropertyModel model)
        {
            Model = model;
            View = new PropertyView(model, this);
        }

        public void OnSelectLayerInDropdown(Section selectedSection)
        {
            if (selectedSection == null)
            {
                return;
            }
            Model.Selected = selectedSection;
            View.UpdateView();
        }
    }
}