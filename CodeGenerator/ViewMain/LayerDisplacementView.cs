using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class LayerDisplacementView : LayerAbstractDetailView
    {
        private readonly LayerDisplacementViewModel _vm;

        public LayerDisplacementView(LayerDisplacementViewModel vm)
        {
            _vm = vm;

            var layout = new DynamicLayout()
            {
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
                Spacing = new Size(5, 1)
            };
            
            addProperty(layout, _vm, "Ux", "Ux", "0");
            addProperty(layout, _vm, "Uy", "Uy", "0");
            addProperty(layout, _vm, "Uz", "Uz", "0");
            addProperty(layout, _vm, "Rotx", "Rotx", "0");
            addProperty(layout, _vm, "Roty", "Roty", "0");
            addProperty(layout, _vm, "Rotz", "Rotz", "0");

            Add(new SelectionView("Displacements", new List<string>() {"General Displacement"},
                new List<Control>() {layout}));
        }
    }
}