using System;
using System.Collections.Generic;
using System.Reflection;
using Eto.Forms;

namespace CodeGenerator
{
    public abstract class LayerAbstractDetailView: DynamicLayout
    {
        public void addProperty(DynamicLayout dynamicLayout, object _vm, string label, string propName, string defaultVal = "")
        {
            var tb = new TextBox();
            tb.Bind<string>("Text", _vm, propName, DualBindingMode.TwoWay);
            dynamicLayout.Add(TableLayout.HorizontalScaled(new Label {Text = label}, tb));
            
            Type myType = _vm.GetType();
            var propertyInfo = myType.GetProperty(propName);
            if (propertyInfo != null)
            {
                var v = propertyInfo.GetValue(_vm);
                if (v == null || v == "")
                {
                    tb.Text = defaultVal;
                }
                Rhino.RhinoApp.WriteLine("{0}, {1}: {2}", propertyInfo, propertyInfo.Name, v);
            }
            
            //
            // foreach (PropertyInfo prop in props)
            // {   
            //     Rhino.RhinoApp.WriteLine("{0}", prop.ToString());
            //     object propValue = prop.GetValue(_vm, null);
            //     Rhino.RhinoApp.WriteLine("{0}", propValue);
            //
            //     // Do something with propValue
            // }
            // var result = _vm.GetType().GetField(propName)?.GetValue(_vm);
            // if (result == null || result.ToString() == "")
            // {
            //     tb.Text = defaultVal;
            // }

            // tb.TextBinding.Bind(() => (string) "abc", val =>
            // {
            //     Rhino.RhinoApp.WriteLine("{0}", val);
            // });
        }
    }
}