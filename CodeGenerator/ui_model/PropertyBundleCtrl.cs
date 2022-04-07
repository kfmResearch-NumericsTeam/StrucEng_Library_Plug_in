using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator.ui_model
{
    public class PropertyBundleCtrl
    {
        public static List<Control> GeneratePropertyViews(Layer currentLayer)
        {
            var sectionModel = SimplePropertyModel.Create("Sections")
                .AddSection(Section.Create("Shell Section", "shell_section")
                    .AddComponent(new TextField("shell_section_thickness", "Thickness", "0")));
            var materialModel = SimplePropertyModel.Create("Materials")
                .AddSection(Section.Create("Elastic", "materials_elastic")
                    .AddComponent(new TextField("materials_elastic_E", "E", "33700"))
                    .AddComponent(new TextField("materials_elastic_v", "v", "0.0"))
                    .AddComponent(new TextField("materials_elastic_p", "p", "2500/10**9")));
            var displacementModel = SimplePropertyModel.Create("Displacements")
                .AddSection(Section.Create("Displacement", "displacements_displacement")
                    .AddComponent(new TextField("set_displacement_ux", "ux", "0"))
                    .AddComponent(new TextField("set_displacement_uy", "uy", "0"))
                    .AddComponent(new TextField("set_displacement_uz", "uz", "0"))
                    .AddComponent(new TextField("set_displacement_rotx", "rotx", "0"))
                    .AddComponent(new TextField("set_displacement_roty", "roty", "0"))
                    .AddComponent(new TextField("set_displacement_rotz", "rotz", "0"))
                );

            sectionModel.Selected = currentLayer?.ElementProperty?.Section ?? null;
            materialModel.Selected = currentLayer?.ElementProperty?.Material ?? null;
            displacementModel.Selected = currentLayer?.SetProperty?.Displacement ?? null;

            var sectionCtrl = new PropertyCtrl(sectionModel);
            var materialCtrl = new PropertyCtrl(materialModel);
            var displacementCtrl = new PropertyCtrl(displacementModel);

            sectionCtrl.CallbackOnModelSelected = (selection) =>
            {
                if (currentLayer?.ElementProperty != null)
                {
                    currentLayer.ElementProperty.Section = selection;
                }

                return true;
            };
            materialCtrl.CallbackOnModelSelected = (selection) =>
            {
                if (currentLayer?.ElementProperty != null)
                {
                    currentLayer.ElementProperty.Material = selection;
                }

                return true;
            };
            displacementCtrl.CallbackOnModelSelected = (selection) =>
            {
                if (currentLayer?.SetProperty != null)
                {
                    currentLayer.SetProperty.Displacement = selection;
                }

                return true;
            };

            List<Control> views = new List<Control>();
            views.Add(sectionCtrl.View.getUi());
            views.Add(materialCtrl.View.getUi());
            views.Add(displacementCtrl.View.getUi());
            return views;
        }

        public static Layout GeneratePropertyViewsControl(Layer currentLayer)
        {
            var layout = new TableLayout
            {
                Padding = new Padding(0),
                Spacing = new Size(5, 1),
            };

            foreach (var ui in GeneratePropertyViews(currentLayer))
            {
                layout.Rows.Add(ui);
            }

            return layout;
        }
    }
}