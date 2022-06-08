using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib
{
    ///
    ///<summary>
    /// A simple dialog which presents a list of layers to select.
    /// Selected layers are in SelectedLayers.
    ///</summary>
    ///  
    public class SelectLayerDialog : Dialog<DialogResult>
    {
        public List<Layer> SelectedLayers { get; } = new List<Layer>();


        private void NoLayers(DynamicLayout layout)
        {
            layout.AddRow(new Label() {Text = "No layers found. Add layers first."});
        }

        private Dictionary<CheckBox, Layer> HasLayers(DynamicLayout layout, List<Layer> layers)
        {
            layout.AddRow(new Label() {Text = "Select layers for load:"});
            Dictionary<CheckBox, Layer> cbMap = new Dictionary<CheckBox, Layer>();
            foreach (var l in layers)
            {
                CheckBox c = new CheckBox();
                cbMap[c] = l;
                c.Text = l.GetName();
                layout.AddRow(c);
                c.CheckedChanged += (sender, args) =>
                {
                    try
                    {
                        var cb = (CheckBox) sender;
                        bool check = cb.Checked != null && (bool) cb.Checked;
                        RhinoUtils.SelectLayerByName(RhinoDoc.ActiveDoc, cb.Text, true, check);
                    }
                    catch (Exception e)
                    {
                        // XXX: Preselect layers for visual aid, ignore on error
                    }
                };
            }

            return cbMap;
        }

        public SelectLayerDialog(List<Layer> layers)
        {
            Padding = new Padding(15) { };
            Title = "Select layers to connect with load";
            DynamicLayout layout = new DynamicLayout();
            layout.Spacing = new Size(10, 10);
            layout.Padding = new Padding() {Top = 10, Bottom = 10, Left = 10, Right = 40};


            var cbMap = new Dictionary<CheckBox, Layer>();
            if (layers == null || layers.Count == 0)
            {
                NoLayers(layout);
            }
            else
            {
                cbMap = HasLayers(layout, layers);
            }


            var button = new Button();
            button.Text = "Ok";
            button.Click += (sender, e) =>
            {
                foreach (var cb in cbMap)
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
        }


        /// <summary>
        /// Create UI component to integrate into load view 
        /// </summary>
        public static Control CreateUiElement(ref Button button, ref TextBox label)
        {
            label = new TextBox();
            label.ReadOnly = true;
            label.AutoSelectMode = AutoSelectMode.Always;
            label.PlaceholderText = "No Layers connected";
            return new TableLayout
            {
                Spacing = new Size(5, 10),
                Padding = new Padding()
                {
                    Bottom = 10,
                },
                Rows =
                {
                    new TableRow
                    {
                        ScaleHeight = false, Cells =
                        {
                            new TableCell((label), true),
                            TableLayout.AutoSized(
                                button = new Button {Text = "Connect...", Enabled = true})
                        }
                    },
                }
            };
        }
    }
}