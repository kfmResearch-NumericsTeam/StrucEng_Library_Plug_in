using System;
using System.Collections.Generic;
using System.Reflection;
using Eto.Forms;

namespace CodeGenerator
{
    /// <summary>
    /// Abstract class with utility code to renders a single row for a property.
    /// The property contains a Label and text field.
    /// </summary>
    public abstract class LayerAbstractDetailView : DynamicLayout
    {
        public void addProperty(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName,
            string defaultVal = "")
        {
            var tb = new TextBox();
            tb.AutoSelectMode = AutoSelectMode.OnFocus;
            tb.Bind<string>("Text", vm, propName, DualBindingMode.TwoWay);
            dynamicLayout.Add(TableLayout.HorizontalScaled(new Label {Text = label}, tb));

            Type myType = vm.GetType();
            var propertyInfo = myType.GetProperty(propName);
            if (propertyInfo != null)
            {
                var v = propertyInfo.GetValue(vm);
                if (v == null || (string) v == "")
                {
                    tb.Text = defaultVal;
                }
            }
        }
    }
}