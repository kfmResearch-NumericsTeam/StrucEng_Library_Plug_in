using System.Collections.Generic;
using System.Collections.ObjectModel;
using StrucEngLib.Model;

namespace StrucEngLib.ViewMain.Step
{
    /// <summary>
    /// Vm to group entries which belong to the same Order/ Step
    /// </summary>
    public class AggregatedStepViewModel : ViewModelBase
    {
        public Model.Step Model { get; }
        public string Order { get; }

        public ObservableCollection<StepViewModel> Steps { get; }

        public AggregatedStepViewModel(Model.Step model, StepViewModel initalEntry, string order)
        {
            Model = model;
            Steps = new ObservableCollection<StepViewModel> {initalEntry};
            Order = order;
        }

        public AggregatedStepViewModel(Model.Step model, List<StepViewModel> entries, string order)
        {
            Model = model;
            Steps = new ObservableCollection<StepViewModel>(entries);
            Order = order;
        }

        public override void UpdateModel()
        {
            Model.Entries.Clear();
            foreach (var s in Steps)
            {
                s.UpdateModel();
                Model.Entries.Add(s.Model);
            }

            Model.Order = Order;
        }
    }
}