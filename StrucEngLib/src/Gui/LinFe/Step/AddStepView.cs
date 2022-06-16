using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib.Step
{
    /// <summary>Simple View to select entries to group into a step</summary>
    public class AddStepView : Dialog<DialogResult>
    {
        public List<StepEntry> SelectedEntries = new List<StepEntry>();

        public AddStepView(string stepId, List<KeyValuePair<string, StepEntry>> entries)
        {
            SelectedEntries = new List<StepEntry>();
            Padding = new Padding(15) { };
            DynamicLayout layout = new DynamicLayout();
            layout.Spacing = new Size(10, 10);
            layout.Padding = new Padding() {Top = 10, Bottom = 10, Left = 10, Right = 40};
            Title = "Add or modify a step";
            layout.AddRow(new Label() {Text = "Select entries to group into step " + stepId});


            if (entries.Count == 0)
            {
                layout.AddRow(new Label()
                {
                    Text = "No entries found. Add layer or load first."
                });
            }

            var selectionMap = new Dictionary<CheckBox, StepEntry>();
            foreach (var l in entries)
            {
                var c = new CheckBox();
                selectionMap[c] = l.Value;
                c.Text = l.Key;
                layout.AddRow(c);
            }

            var button = new Button();
            button.Text = "Ok";
            button.Click += (sender, e) =>
            {
                foreach (var cb in selectionMap)
                {
                    if (cb.Key.Checked == true)
                    {
                        SelectedEntries.Add(cb.Value);
                    }
                }

                Close(DialogResult.Ok);
            };

            layout.AddSpace();
            layout.AddSeparateRow(TableLayout.AutoSized(button));
            Content = layout;
        }
    }
}