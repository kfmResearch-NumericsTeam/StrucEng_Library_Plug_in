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
            };
        }

        public void UpdateView()
        {
            Rhino.RhinoApp.WriteLine("Updating view");
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

            _propertyLayout = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            sectionLayout.Rows.Add(_propertyLayout);

            _dbSectionSelection.ItemTextBinding = Binding.Property((Section t) => t.Label);
            _dbSectionSelection.DataStore = Model.Sections;
            
            _dbSectionSelection.SelectedValueChanged += (sender, e) =>
            {
                Rhino.RhinoApp.WriteLine("Selected Value Chagned: {0}, {1}, {2}", e, sender,
                    _dbSectionSelection.SelectedValue);
                _propertyLayout.Content = new PropertyLayout((Section) _dbSectionSelection.SelectedValue);
            };

            // _dbSectionSelection.SelectedIndex = 0;
            // Model.Selected = Model.Sections[0];

            // _propertyLayout = new DynamicLayout
            // {
            //     Padding = new Padding(5),
            //     Spacing = new Size(5, 5),
            // };
            //
            // // The text properties
            // if (Model.Selected != null)
            // {
            //     Rhino.RhinoApp.WriteLine("Model.Selected != null");
            //     var fieldLayout = new TableLayout
            //     {
            //         Padding = new Padding {Top = 0, Left = 10, Bottom = 0, Right = 0},
            //         Spacing = new Size(5, 1),
            //     };
            //
            //     foreach (var component in Model.Selected.Type.Components)
            //     {
            //         var tb = new TextBox();
            //         tb.TextBinding.Bind(() => (string) component.Value ?? (string) component.Default,
            //             val => component.Value = val);
            //         fieldLayout.Rows.Add(TableLayout.HorizontalScaled(new Label {Text = component.Label}, tb));
            //         Rhino.RhinoApp.WriteLine("{0} -> {1}", component.Label, (string) component.Value);
            //     }
            //
            //     _propertyLayout.Content = fieldLayout;
            //     layout.Add(fieldLayout);


            // _glueController();
            // GeneratePropertyLayout();

            return layout;
        }

        class PropertyLayout : TableLayout
        {
            public PropertyLayout(Section section)
            {
                Padding = new Padding {Top = 0, Left = 10, Bottom = 0, Right = 0};
                Spacing = new Size(5, 1);

                foreach (var component in section.Components)
                {
                    var tb = new TextBox();
                    tb.TextBinding.Bind(() => (string) component.Value ?? (string) component.Default,
                        val => component.Value = val);

                    Rows.Add(TableLayout.HorizontalScaled(new Label {Text = component.Label}, tb));
                    Rhino.RhinoApp.WriteLine("{0} -> {1}", component.Label, (string) component.Value);
                }
            }
        }

        // XXX: What about unbindings?
        private void GeneratePropertyLayout()
        {
            // // The text properties
            // if (Model.Selected != null)
            // {
            //     Rhino.RhinoApp.WriteLine("GeneratePropertyLayout != null");
            //     var fieldLayout = new TableLayout
            //     {
            //         Padding = new Padding {Top = 0, Left = 10, Bottom = 0, Right = 0},
            //         Spacing = new Size(5, 1),
            //     };
            //     
            //     foreach (var component in Model.Selected.Components)
            //     {
            //         var tb = new TextBox();
            //         tb.TextBinding.Bind(() => (string) component.Value ?? (string) component.Default,
            //             val => component.Value = val);
            //         fieldLayout.Rows.Add(TableLayout.HorizontalScaled(new Label {Text = component.Label}, tb));
            //         Rhino.RhinoApp.WriteLine("{0} -> {1}", component.Label, (string) component.Value);
            //     }
            //
            //     _propertyLayout.Content = fieldLayout;
            // }
        }
    }
}