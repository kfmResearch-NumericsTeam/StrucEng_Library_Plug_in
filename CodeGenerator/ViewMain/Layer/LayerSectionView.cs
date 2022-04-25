using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class LayerSectionView : LayerAbstractDetailView
    {
        private readonly LayerSectionViewModel _vm;

        public LayerSectionView(LayerSectionViewModel vm)
        {
            _vm = vm;

            var layout = new DynamicLayout()
            {
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
                Spacing = new Size(5, 1)
            };
            addProperty(layout, _vm, "Thickness", "Thickness");
            Add(new SelectionView("Sections", new List<string>() {"Shell Section"}, new List<Control>() {layout}));
        }
    }
}