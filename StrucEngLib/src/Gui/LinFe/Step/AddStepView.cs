using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib.Step
{
    /// <summary>Simple View to select entries to group into a step</summary>
    public class AddStepView : Dialog<DialogResult>
    {
        private readonly string _stepId;
        private readonly List<KeyValuePair<string, StepEntry>> _entries;
        private Button _button;

        public List<StepEntry> SelectedEntries = new List<StepEntry>();
        private Dictionary<CheckBox, StepEntry> _selectionMap;

        private void BuildGui()
        {
            // entries: <step description, step entry>
            SelectedEntries = new List<StepEntry>();
            Padding = new Padding(15) { };
            DynamicLayout layout = new DynamicLayout();
            layout.Spacing = new Size(10, 10);
            layout.Padding = new Padding() {Top = 10, Bottom = 10, Left = 10, Right = 40};
            Title = "Add or modify a step";
            layout.AddRow(new Label() {Text = "Select entries to group into step " + _stepId});


            if (_entries.Count == 0)
            {
                layout.AddRow(new Label()
                {
                    Text = "No entries found. Add layer or load first."
                });
            }

            _selectionMap = new Dictionary<CheckBox, StepEntry>();
            foreach (var l in _entries)
            {
                var c = new CheckBox();
                _selectionMap[c] = l.Value;
                c.Text = l.Key;
                layout.AddRow(c);
                BindCheckBox(c, l);
            }

            _button = new Button();
            _button.Text = "Ok";
            layout.AddSpace();
            layout.AddSeparateRow(TableLayout.AutoSized(_button));
            Content = layout;

            // Init selection
            RhinoUtils.UnSelectAll(RhinoDoc.ActiveDoc);
        }

        private void BindCheckBox(CheckBox cb, KeyValuePair<string, StepEntry> keypair)
        {
            cb.CheckedChanged += (sender, args) =>
            {
                RhinoUtils.UnSelectAll(RhinoDoc.ActiveDoc);
                List<string> names = new List<string>();
                foreach (var kvp in _selectionMap)
                {
                    if (kvp.Key.Checked == true)
                    {
                        names.AddRange(LayerNames(kvp.Value));
                    }
                }
                RhinoUtils.SelectLayerByNames(RhinoDoc.ActiveDoc, names);
            };
        }

        private void BindGui()
        {
            _button.Click += (sender, e) =>
            {
                foreach (var cb in _selectionMap)
                {
                    if (cb.Key.Checked == true)
                    {
                        SelectedEntries.Add(cb.Value);
                    }
                }

                Close(DialogResult.Ok);
            };
        }

        private IEnumerable<string> LayerNames(StepEntry e)
        {
            List<string> names = new List<string>();
            if (e.Type is StepType.Load)
            {
                var load = (Model.Load) e.Value;
                names.AddRange(load.Layers.Select(l => l.GetName()));
            }
            else if (e.Type is StepType.Set)
            {
                var set = (Model.Set) e.Value;
                names.Add(set.Name);
            }

            return names;
        }

        public AddStepView(string stepId, List<KeyValuePair<string, StepEntry>> entries)
        {
            _stepId = stepId;
            _entries = entries;

            BuildGui();
            BindGui();
        }
    }
}