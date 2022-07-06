using System.Collections.Generic;

namespace StrucEngLib.Model.Sm
{
    /// <summary>Model data for sandwich</summary>
    public class SandwichModel
    {
        public string FileName { get; set; }
        public List<SandwichProperty> AdditionalProperties { get; set; } = new List<SandwichProperty>();
        public List<SmAnalysisSetting> AnalysisSettings { get; set; } = new List<SmAnalysisSetting>();
    }
}