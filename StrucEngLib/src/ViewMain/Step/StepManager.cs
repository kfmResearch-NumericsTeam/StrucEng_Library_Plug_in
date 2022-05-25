using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib.ViewMain.Step
{
    /// <summary>
    /// Step Manager.
    /// Several entries (steps) can be grouped together into an aggregated step
    /// The Step Manager keeps track of which steps belong together, and rewires the steps if they
    /// are changed to belong to another step.
    ///
    /// Other view models should listen on <AggregatedStepViewModel> AggregatedSteps
    /// to attach meta information to an aggregated step.
    ///
    /// A StepViewModel severs a single entry which can be assigned to a step
    /// A AggregatedStepViewModel groups all entries with the same step. It is updated as order ids are changed.
    /// </summary>
    public class StepManager : ViewModelBase
    {
        private readonly MainViewModel _vm;
        public static string StepNameExclude { get; } = "Excluded";

        public ObservableCollection<AggregatedStepViewModel> AggregatedSteps;

        public ObservableCollection<StepViewModel> Steps;

        public ObservableCollection<string> StepNames;

        public override void UpdateModel()
        {
            foreach (var ags in AggregatedSteps)
            {
                ags.UpdateModel();
            }
        }

        public List<Model.Step> ExportModel()
        {
            List<Model.Step> steps = new List<Model.Step>();
            foreach (var avm in AggregatedSteps)
            {
                avm.UpdateModel();
                steps.Add(avm.Model);
            }

            return steps;
        }

        public StepManager(MainViewModel vm)
        {
            _vm = vm;
            AggregatedSteps = new ObservableCollection<AggregatedStepViewModel>();
            Steps = new ObservableCollection<StepViewModel>();
            StepNames = new ObservableCollection<string>() {StepNameExclude};
        }

        protected StepViewModel StepViewModelByObject(object o)
        {
            foreach (var s in Steps)
            {
                if (s.Model.Value == o)
                {
                    return s;
                }
            }

            return null;
        }

        protected AggregatedStepViewModel AggregatedStepViewModelByStepViewModel(StepViewModel vm)
        {
            foreach (var s in AggregatedSteps)
            {
                foreach (var step in s.Steps)
                {
                    if (step == vm)
                    {
                        return s;
                    }
                }
            }

            return null;
        }

        protected AggregatedStepViewModel AggregatedStepByOrder(string order)
        {
            var vm = AggregatedSteps.FirstOrDefault(s => s.Order == order);
            return vm;
        }

        private void AddIfNotContains<T>(ObservableCollection<T> c, T value)
        {
            if (!c.Contains(value))
            {
                c.Add(value);
            }
        }

        public void Clear()
        {
            Steps.Clear();
            AggregatedSteps.Clear();
            StepNames.Clear();
            StepNames.Add(StepNameExclude);
        }

        protected void RemoveFromAggregation(StepViewModel step)
        {
            foreach (var a in AggregatedSteps)
            {
                if (a.Steps.Contains(step))
                {
                    a.Steps.Remove(step);
                    return;
                }
            }
        }

        public string AddNewStepId()
        {
            var max = 0;
            foreach (var s in AggregatedSteps)
            {
                int.TryParse(s.Order, out var order);
                max = Math.Max(max, order);
            }

            var id = max + 1;
            AddIfNotContains(StepNames, id.ToString());
            return id.ToString();
        }

        public void RemoveStep(StepType type, object step)
        {
            var sVm = StepViewModelByObject(step);
            if (sVm != null)
            {
                var aVm = AggregatedStepViewModelByStepViewModel(sVm);
                if (aVm != null)
                {
                    if (aVm.Steps.Count <= 1)
                    {
                        AggregatedSteps.Remove(aVm);
                    }
                }

                Steps.Remove(sVm);
            }
        }

        private StepViewModel NewStepEntryVm(StepEntry entryStep, string order)
        {
            var stepVm = new StepViewModel(entryStep, StepNameExclude);
            stepVm.PropertyChanged += (sender, args) =>
            {
                // Logic to change step entry into other bucket if order changed
                if (args.PropertyName == nameof(stepVm.Order))
                {
                    RemoveFromAggregation(stepVm);
                    var vm = AggregatedStepByOrder(stepVm.Order);
                    vm?.Steps.Add(stepVm);
                }
            };
            return stepVm;
        }

        public void NewAggregationStep(Model.Step model, StepEntry entryStep)
        {
            var stepVm = NewStepEntryVm(entryStep, StepNameExclude);
            Steps.Add(stepVm);

            var order = AddNewStepId();
            var agg = new AggregatedStepViewModel(model, stepVm, order);
            AggregatedSteps.Add(agg);
        }

        public void ExistingAggregateStep(Model.Step model)
        {
            // for existing data, all steps in Entries have currently the same order
            List<StepViewModel> entryVms = new List<StepViewModel>();
            foreach (var step in model.Entries)
            {
                var eVm = NewStepEntryVm(step, model.Order);
                entryVms.Add(eVm);
                Steps.Add(eVm);
            }

            var aVm = new AggregatedStepViewModel(model, entryVms, model.Order);
            AddIfNotContains(StepNames, model.Order);
            AggregatedSteps.Add(aVm);
        }
    }
}