using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace StrucEngLib.Model
{
    /// <summary>Analysis Settings</summary>
    public class AnalysisSetting
    {
        public string StepId { get; set; } = null;
        public bool Include { get; set; }
        public bool Rf { get; set; }
        public bool Rm { get; set; }
        public bool U { get; set; }
        public bool Ur { get; set; }
        public bool Cf { get; set; }
        public bool Cm { get; set; }
        
        public bool SectionMoments { get; set; }
        public bool ShellForces { get; set; }
        
        public bool AnySettingEnabled =>
            Include && new List<bool?>() {Rf, Rm, U, Ur, Cf, Cm, SectionMoments, ShellForces}.Any(f => f == true);


        public override string ToString()
        {
            return $"{nameof(StepId)}: {StepId}, {nameof(Include)}: {Include}, {nameof(Rf)}: {Rf}," +
                   $" {nameof(Rm)}: {Rm}, {nameof(U)}: {U}, {nameof(Ur)}: {Ur}, {nameof(Cf)}: {Cf}, " +
                   $"{nameof(Cm)}: {Cm}, {nameof(SectionMoments)}: {SectionMoments}, " +
                   $"{nameof(ShellForces)}: {ShellForces}";
        }
    }
}