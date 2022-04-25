using System;

namespace CodeGenerator.Model
{
    public enum StepType
    {
        Load,
        Displacement
    };


    public class Step
    {
        public int Order { get; set; }

        public StepType StepType { get; set; }

        public Load Load { get; set; }
        public Displacement Displacement { get; set; }

        public Step(StepType type)
        {
            StepType = type;
        }
    }
}