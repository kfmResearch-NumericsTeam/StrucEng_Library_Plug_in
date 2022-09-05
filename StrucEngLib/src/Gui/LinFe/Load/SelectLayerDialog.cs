using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Load
{
    using Layer = StrucEngLib.Model.Layer;

    ///
    ///<summary>
    /// A simple dialog which presents a list of layers to select.
    /// Selected layers are in SelectedLayers.
    ///</summary>
    ///  
    public class SelectLayerDialog : Dialog<DialogResult>
    {
        public List<Layer> SelectedLayers { get; } = new List<Layer>();
        private readonly Dictionary<CheckBox, Layer> _checkboxMap = new Dictionary<CheckBox, Layer>();

        public SelectLayerDialog(List<Layer> layers)
        {
            Padding = new Padding(15) { };
            Title = "Select layers to connect with load";
            DynamicLayout layout = new DynamicLayout();
            layout.Spacing = new Size(10, 10);
            layout.Padding = new Padding() {Top = 10, Bottom = 10, Left = 10, Right = 40};
            if (layers == null || layers.Count == 0)
            {
                NoLayers(layout);
            }
            else
            {
                AddCheckboxes(layout, layers);
            }


            var button = new Button();
            button.Text = "Ok";
            button.Click += (sender, e) =>
            {
                foreach (var cb in _checkboxMap)
                {
                    if (cb.Key.Checked == true)
                    {
                        SelectedLayers.Add(cb.Value);
                    }
                }

                Close(DialogResult.Ok);
            };

            layout.AddSpace();
            layout.AddSeparateRow(TableLayout.AutoSized(button));
            Content = layout;
            RhinoUtils.UnSelectAll(RhinoDoc.ActiveDoc);
        }

        private void NoLayers(DynamicLayout layout)
        {
            layout.AddRow(new Label() {Text = "No layers found. Add layers first."});
        }

        private void AddCheckboxes(DynamicLayout layout, List<Layer> layers)
        {
            _checkboxMap.Clear();
            layout.AddRow(new Label() {Text = "Select layers for load:"});

            foreach (var l in layers)
            {
                var c = new CheckBox();
                _checkboxMap[c] = l;
                c.Text = l.GetName();
                layout.AddRow(c);
                c.CheckedChanged += (sender, args) =>
                {
                    try
                    {
                        var names = new List<string>();
                        foreach (var kv in _checkboxMap)
                        {
                            var cb =kv.Key;
                            if (cb.Checked != null && (bool) cb.Checked)
                            {
                                names.Add(kv.Value.GetName());
                            }
                        }
                        RhinoUtils.SelectLayerByNames(RhinoDoc.ActiveDoc, names);
                    }
                    catch (Exception)
                    {
                        // XXX: Preselect layers for visual aid, ignore on error
                    }
                };
            }
        }
    }
}