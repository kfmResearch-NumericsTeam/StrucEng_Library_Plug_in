using System.Collections.Generic;
using CodeGenerator.ui_model;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public class PropertyView
    {
        public SimplePropertyModel Model;
        public PropertyCtrl Ctrl;

        private bool _ctrlGlued = false;

        public PropertyView(SimplePropertyModel model, PropertyCtrl ctrl)
        {
            Model = model;
            Ctrl = ctrl;
        }

        private DropDown _dbSectionSelection;
        private DynamicLayout _propertyLayout;

        private void _glueController()
        {
            if (_ctrlGlued)
            {
                return;
            }

            _ctrlGlued = true;

            _dbSectionSelection.SelectedIndexChanged += (sender, e) =>
            {
                Ctrl.OnSelectLayerInDropdown((Section) _dbSectionSelection.SelectedValue);
                Rhino.RhinoApp.WriteLine("update dropdown");
            };
        }

        public void UpdateView()
        {
            GeneratePropertyLayout();
        }

        public Control getUi()
        {
            var layout = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            var sectionLayout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            layout.Add(sectionLayout);

            sectionLayout.Rows.Add(new ViewSeparator() {Text = Model.Label});
            sectionLayout.Rows.Add(TableLayout.HorizontalScaled((_dbSectionSelection = new DropDown { })));


            _dbSectionSelection.ItemTextBinding = Binding.Property((Section t) => t.Label);
            _dbSectionSelection.DataStore = Model.Sections;

            _propertyLayout = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            layout.Add(_propertyLayout);

            _glueController();
            GeneratePropertyLayout();
            return layout;
        }

        // XXX: What about unbindings?
        private void GeneratePropertyLayout()
        {
            Rhino.RhinoApp.WriteLine("GeneratePropertyLayout");
            // The text properties
            if (Model.Selected != null)
            {
                Rhino.RhinoApp.WriteLine("GeneratePropertyLayout != null");
                var fieldLayout = new TableLayout
                {
                    Padding = new Padding {Top = 0, Left = 10, Bottom = 0, Right = 0},
                    Spacing = new Size(5, 1),
                };
                foreach (var component in Model.Selected.Components)
                {
                    var tb = new TextBox();
                    tb.TextBinding.Bind(() => (string) component.Value ?? (string) component.Default,
                        val => component.Value = val);
                    fieldLayout.Rows.Add(TableLayout.HorizontalScaled(new Label {Text = component.Label}, tb));
                }

                _propertyLayout.Content = fieldLayout;
            }
        }
    }
}