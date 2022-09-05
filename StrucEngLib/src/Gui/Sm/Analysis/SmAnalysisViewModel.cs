
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Rhino;
using StrucEngLib.Model;
using StrucEngLib.Model.Sm;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.Sm
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
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged -= SelectedItemIncludeChanged;
                }

                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItemVisible));
                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged += SelectedItemIncludeChanged;
                }

                OnPropertyChanged();
            }
        }

        private void SelectedItemIncludeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem.Include))
            {
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

        private bool ViewModelContainsStepName(string name)
        {
            return AnalysisViewItems.Any(vm => vm.StepName.Equals(name));
        }

        private bool ModelContainsStepName(string name)
        {
            return _vm.Workbench.Steps != null &&
                   _vm.Workbench.Steps.Any(
                       s => s.Order.Equals(name)
                            && s.Setting != null
                            && s.Setting.Include
                            && !s.ContainsType(StepType.Set));
        }

        public sealed override void UpdateViewModel()
        {
            AnalysisViewItems.Clear();
            // XXX: We only add steps which are also defined in LinFE and are "included"
            // and contain no 'set' layer.

            // Add existing Analysis settings from sandwich model
            _vm.Workbench?.SandwichModel?.AnalysisSettings?.ForEach(m =>
            {
                if (ModelContainsStepName(m.Step.Order))
                {
                    AnalysisViewItems.Add(new SmAnalysisItemViewModel(m));
                }
            });

            // Add new analysis setting based of steps
            _vm.Workbench?.Steps?.ForEach(modelStep =>
            {
                if (modelStep.Setting?.Include == true
                    && !modelStep.ContainsType(StepType.Set)
                    && !ViewModelContainsStepName(modelStep.Order))
                {
                    AnalysisViewItems.Add(SmAnalysisItemViewModel.CreateNew(new SmAnalysisSetting()
                    {
                        Step = modelStep
                    }));
                }
            });
        }

        public void RhinoSelectStep()
        {
            if (SelectedItem == null) return;
            RhinoUtils.SelectLayerByNames(RhinoDoc.ActiveDoc, SelectedItem.Model.Step.LayerNames());
        }
    }
}