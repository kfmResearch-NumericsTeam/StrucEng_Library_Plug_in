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
        public static Control TextInputWithDataContextBinding(IndirectBinding<string> binding, string defaultValue = "")
        {
            // Usage: IndirectBinding<string> binding = Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichBot);
            var tb = new ComboBoxWithMemory();
            var ctx = tb.BindDataContext(srcWidget => srcWidget.Text, binding);
            tb.DataContextChanged += (sender, args) =>
            {
                var d = ctx.Destination.DataValue;
                if (d == null || d == String.Empty)
                {
                    ctx.Destination.DataValue = defaultValue;
                }
            };

            return tb;
        }

        public static Control CheckboxInputWithDataContextBinding(IndirectBinding<bool?> binding)
        {
            var cb = new CheckBox();
            var ctx = cb.BindDataContext<CheckBox, bool?>(srcWidget => srcWidget.Checked, binding);
            return cb;
        }

        public static void AddLabelTextRow(
            DynamicLayout dynamicLayout,
            Control label,
            IndirectBinding<string> binding,
            string defaultValue = ""
        )
        {
            var tb = TextInputWithDataContextBinding(binding, defaultValue);
            FocusOnClick(label, tb);
            dynamicLayout.Add(TableLayout.HorizontalScaled(label, tb));
        }

        public static void AddLabelTextRow(
            DynamicLayout dynamicLayout,
            string label,
            IndirectBinding<string> binding,
            string defaultValue = ""
        )
        {
            Label l;
            var tb = TextInputWithDataContextBinding(binding, defaultValue);
            FocusOnClick(l = new Label {Text = label}, tb);
            dynamicLayout.Add(TableLayout.HorizontalScaled(l, tb));
        }

        public static void AddLabelCheckboxRow(
            DynamicLayout dynamicLayout,
            string label,
            IndirectBinding<bool?> binding
        )
        {
            Label l;
            var tb = CheckboxInputWithDataContextBinding(binding);
            dynamicLayout.Add(TableLayout.HorizontalScaled(l = new Label {Text = label}, tb));
            FocusOnClick(l, tb);
        }

        public static void AddLabelTextRow(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName,
            string defaultVal = "")
        {
            Label l;
            var tb = new ComboBoxWithMemory(propName);
            tb.Bind<string>(nameof(tb.Text), vm, propName, DualBindingMode.TwoWay);
            dynamicLayout.Add(TableLayout.HorizontalScaled(l = new Label {Text = label}, tb));
            FocusOnClick(l, tb);

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

        public static void AddLabelCheckboxRow(
            DynamicLayout dynamicLayout,
            object vm,
            string label,
            string propName)
        {
            var cb = new CheckBox();
            var l = new Label {Text = label};
            l.MouseDown += (sender, args) => { cb.Checked = !cb.Checked; };
            cb.Bind<bool>(nameof(cb.Checked), vm, propName, DualBindingMode.TwoWay);
            dynamicLayout.Add(TableLayout.HorizontalScaled(l, cb));
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
            Label l;
            var tb = new DropDown();
            tb.DataStore = items;

            tb.Bind<string>(nameof(tb.SelectedValue), vm, propName, DualBindingMode.TwoWay);
            tb.SelectedIndex = defaultSelectionIndex;
            dynamicLayout.Add(TableLayout.HorizontalScaled(l = new Label {Text = label}, tb));
            FocusOnClick(l, tb);
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

        protected static void FocusOnClick(Control toClick, Control toFocus)
        {
            toClick.MouseDown += (sender, args) => { toFocus.Focus(); };
        }
    }
}