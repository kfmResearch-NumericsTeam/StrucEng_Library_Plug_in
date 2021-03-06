using System.Collections.Generic;
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
        
        public bool ShellMoments { get; set; }
        public bool SectionMoments { get; set; }
        public bool ShellForces { get; set; }
        public bool SectionForces { get; set; }
        public bool SpringForces { get; set; }

        public override string ToString()
        {
            return $"{nameof(StepId)}: {StepId}, {nameof(Include)}: {Include}, {nameof(Rf)}: {Rf}, {nameof(Rm)}: {Rm}, " +
                   $"{nameof(U)}: {U}, {nameof(Ur)}: {Ur}, {nameof(Cf)}: {Cf}, {nameof(Cm)}: {Cm}, {nameof(ShellMoments)}: " +
                   $"{ShellMoments}, {nameof(SectionMoments)}: {SectionMoments}, {nameof(ShellForces)}: {ShellForces}, " +
                   $"{nameof(SectionForces)}: {SectionForces}, {nameof(SpringForces)}: {SpringForces}";
        }
    }
    
}