using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Rhino;
using StrucEngLib.ViewMain.Step;

namespace StrucEngLib.Analysis
{
    /// <summary>View Model for Compas Analysis Settings </summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;

        public ObservableCollection<AnalysisItemViewModel> AnalysisViewItems { get; }

        private Dictionary<string, AnalysisItemViewModel> _analysisViewItemsMap;

        private AnalysisItemViewModel _selectedItem;

        public bool SelectedItemVisible
        {
            get => SelectedItem != null && SelectedItem.Include == true;
        }

        public AnalysisItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    // XXX: We update SelectedItemVisible if include flag is changed
                    _selectedItem.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(_selectedItem.Include))
                        {
                            OnPropertyChanged(nameof(SelectedItemVisible));
                        }
                    };
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedItemVisible));
            }
        }

        private HashSet<string> stepNames = new HashSet<string>();

        public AnalysisViewModel(MainViewModel vm)
        {
            _vm = vm;
            _analysisViewItemsMap = new Dictionary<string, AnalysisItemViewModel>();
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();
            if (vm.ListLayerVm.Model.AnalysisSettings != null)
            {
                foreach (var s in vm.ListLayerVm.Model.AnalysisSettings)
                {
                    stepNames.Add(s.StepId);
                }
            }

            // vm.ListStepVm.Steps.CollectionChanged += (sender, args) =>
            // {
            //     if (args.NewItems != null)
            //     {
            //         foreach (var i in args.NewItems)
            //         {
            //             var step = (SingleStepViewModel) i;
            //             AnalysisViewItems.Add(new AnalysisItemViewModel
            //             {
            //                 StepName = step.Order
            //             });
            //         }
            //     }
                
                // if (args.OldItems != null)
                // {
                //     foreach (var i in args.OldItems)
                //     {
                //         var step = (SingleStepViewModel) i;
                //         AnalysisViewItems.Add(new AnalysisItemViewModel
                //         {
                //             StepName = step.Order
                //         });
                //     }
                // }
            // };

            // AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 1"});
            // AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 2"});
            // AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 3"});
        }
    }
}