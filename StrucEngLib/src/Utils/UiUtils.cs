using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Views;

namespace StrucEngLib
{
    /// <summary>
    /// Utility code for views
    /// </summary>g
    public class UiUtils
    {
        public static void AddLabelTextRow(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName,
            string defaultVal = "")
        {
            var tb = new ComboBoxWithMemory(propName);

            tb.Bind<string>(nameof(tb.Text), vm, propName, DualBindingMode.TwoWay);
            dynamicLayout.Add(TableLayout.HorizontalScaled(new Label {Text = label}, tb));

            Type myType = vm.GetType();
            var propertyInfo = myType.GetProperty(propName);
            if (propertyInfo != null)
            {
                var v = propertyInfo.GetValue(vm);
                if (v == null || v.ToString() == "")
                {
                    tb.Text = defaultVal;
                }
            }
        }

        public static void AddLabelDropdownRowBoolean(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName,
            bool defaultTrue = true)
        {
            AddLabelDropdownRow(dynamicLayout, vm, label, propName, new[] {"true", "false"},
                defaultTrue ? 0 : 1);
        }

        public static void AddLabelDropdownRow(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName,
            IEnumerable<string> items,
            int defaultSelectionIndex = 0)
        {
            var tb = new DropDown();
            tb.DataStore = items;

            tb.Bind<string>(nameof(tb.SelectedValue), vm, propName, DualBindingMode.TwoWay);
            tb.SelectedIndex = defaultSelectionIndex;
            dynamicLayout.Add(TableLayout.HorizontalScaled(new Label {Text = label}, tb));
        }

        public static Control GenerateTitle(string text)
        {
            var s = new Label().Font.Size;
            return new ViewSeparator()
            {
                Text = text,
                Label =
                {
                    Font = new Font(FontFamilies.Sans, s, FontStyle.None)
                }
            };
        }
    }
}
