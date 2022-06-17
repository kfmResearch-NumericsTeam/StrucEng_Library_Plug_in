using System.Collections.Generic;

namespace StrucEngLib.Model.Sm
{
    /// <summary></summary>
    public class SandwichModel
    {
        public string StepName { get; set; }
        public string AxesScale { get; set; }
        public string DruckzonenIteration { get; set; }
        public string MindestBewehrung { get; set; }
        public List<SandwichProperty> AdditionalProperties { get; set; } = new List<SandwichProperty>();
    }
}