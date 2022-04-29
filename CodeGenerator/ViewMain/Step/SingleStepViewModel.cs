namespace CodeGenerator.ViewMain.Step
{
    /// <summary> VM for a single Step in step view </summary>
    public class SingleStepViewModel : ViewModelBase
    {
        public Model.Step StepModel { get; }

        private string _order;

        public string Order
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged();
                StepModel.Order = _order;
            }
        }

        public string Label => StepModel.GetSummary();

        public SingleStepViewModel(Model.Step step)
        {
            StepModel = step;
            Order = step.Order;
        }
    }
}