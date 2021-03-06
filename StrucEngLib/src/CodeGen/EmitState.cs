using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Rhino.DocObjects.Tables;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>
    /// State Machine to emit python code
    /// </summary>
    public class EmitState
    {
        private int _indent = 0;
        
        public string RemoveSpaces(string id) => id.Replace(" ", "_");
        public string LoadId() => "load_" + LoadIdCounter++;
        public string ElementId(string id) => RemoveSpaces(id) + "_element";
        public string SetId(string id) => RemoveSpaces(id) + "_set";
        public string SectionId(string id) => RemoveSpaces(id) + "_sec";
        public string PropId(string id) => RemoveSpaces(id) + "_prop";
        public string MatElasticId(string id) => RemoveSpaces(id) + "_mat_elastic";
        public string DispId(string id) => RemoveSpaces(id) + "_disp";

        public string CreateStepName(string id) => "step_" + RemoveSpaces(id);

        public string EmitIfNotEmpty(string key, string value, string comma = ",")
            => string.IsNullOrWhiteSpace(value) ? "" : $" {key}={value}{comma}";

        public string PythonBoolean(string val) =>
            (val != null && val.ToLower().Trim().Equals("true")) ? "True" : "False";

        public Workbench Workbench { get; }
        public StringBuilder Buffer { get; } = new StringBuilder();
        public int LoadIdCounter { get; set; } = 0;

        public Dictionary<Model.Layer, string> LayerIds { get; } = new Dictionary<Model.Layer, string>();
        public Dictionary<Model.Layer, string> MaterialIds { get; } = new Dictionary<Model.Layer, string>();
        public Dictionary<Model.Layer, string> SectionIds { get; } = new Dictionary<Model.Layer, string>();
        public Dictionary<Model.Layer, string> PropertyIds { get; } = new Dictionary<Model.Layer, string>();
        public Dictionary<Model.Layer, string> DisplacementIds { get; } = new Dictionary<Model.Layer, string>();
        public Dictionary<Model.Load, string> LoadIds { get; } = new Dictionary<Model.Load, string>();
        public Dictionary<Model.Step, string> StepIds { get; } = new Dictionary<Model.Step, string>();

        public EmitState(Workbench bench)
        {
            Workbench = bench;
            _indent = 0;
        }

        private string GetIndent()
        {
            var b = new StringBuilder();
            for (int i = 0; i < _indent; i++)
            {
                b.Append("    ");
            }

            return b.ToString();
        }

        public void Line(string s)
        {

            Buffer.Append(GetIndent());
            Buffer.Append(s + Environment.NewLine);
        }

        public void CommentLine(string s)
        {
            Buffer.Append(Environment.NewLine);
            Buffer.Append(GetIndent());
            Buffer.Append("# " + s + Environment.NewLine);
        }

        public List<Element> Elements()
        {
            return Workbench.Layers
                .Where(l => l.LayerType == LayerType.ELEMENT).ToList()
                .Select(s => s as Element).ToList();
        }

        public List<Set> Sets()
        {
            return Workbench.Layers
                .Where(l => l.LayerType == LayerType.SET).ToList()
                .Select(s => s as Set).ToList();
        }

        public void IncreaseIndent()
        {
            _indent++;
        }

        public void DecreaseIndent()
        {
            if (_indent == 0)
            {
                throw new Exception("Mismatching indent. Got negative");
            }

            _indent--;
        }
    }
}