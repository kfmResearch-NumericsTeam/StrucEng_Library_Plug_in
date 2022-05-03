using System;
using CodeGenerator.ui_model;
using Eto.Forms;

namespace CodeGenerator

{
    public class PropertyCtrl
    {
        // XXX: Callback when Selected Section Changes
        public Func<Section, Boolean> CallbackOnModelSelected { get; set; }

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

            var res = CallbackOnModelSelected?.Invoke(selectedSection);
            if (res != null && res.Value == false)
            {
                return;
            }

            Model.Selected.Type = selectedSection;
            View.UpdateView();
        }
    }
}