using System;
using System.Collections.ObjectModel;
using System.Linq;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;
using StrucEngLib.Sm;
using StrucEngLib.Step;

namespace StrucEngLib.Sm
{
    /// <summary>View Model for Analysis Output of Sandwich Model</summary>
    public class SmAnalysisViewModel : ViewModelBase
    {
        private readonly SmMainViewModel _vm;

        public ObservableCollection<SmAnalysisItemViewModel> AnalysisViewItems { get; }
        private SmAnalysisItemViewModel _selectedItem;
        public bool SelectedItemVisible => SelectedItem != null && SelectedItem.Include;

        public SmAnalysisItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedItemVisible));
            }
        }

        public SmAnalysisViewModel(SmMainViewModel vm)
        {
            _vm = vm;
            AnalysisViewItems = new ObservableCollection<SmAnalysisItemViewModel>();
            UpdateViewModel();
        }


        public override void UpdateModel()
        {
            _vm.Workbench.SandwichModel?.AnalysisSettings.Clear();
            foreach (var avm in AnalysisViewItems)
            {
                avm.UpdateModel();
                _vm.Workbench.SandwichModel?.AnalysisSettings.Add(avm.Model);
            }
        }

        private bool ContainsViewModelStepName(string name)
        {
            return AnalysisViewItems.Any(vm => vm.StepName.Equals(name));
        }

        private bool ContainsModelStepName(string name)
        {
            return _vm.Workbench.Steps != null &&
                   _vm.Workbench.Steps.Any(s => s.Order.Equals(name)
                                                && s.Setting != null
                                                && s.Setting.Include);
        }

        public sealed override void UpdateViewModel()
        {
            AnalysisViewItems.Clear();
            // We only add steps which are also defined in LinFE and are "included"
            _vm.Workbench?.SandwichModel?.AnalysisSettings?.ForEach(m =>
            {
                if (ContainsModelStepName(m.Step.Order))
                {
                    AnalysisViewItems.Add(new SmAnalysisItemViewModel(m));
                }
            });
            _vm.Workbench?.Steps?.ForEach(s =>
            {
                if (s.Setting?.Include == true && !ContainsViewModelStepName(s.Order))
                {
                    AnalysisViewItems.Add(new SmAnalysisItemViewModel(new SmAnalysisSetting()
                    {
                        Step = s
                    }));
                }
            });
        }
    }
}