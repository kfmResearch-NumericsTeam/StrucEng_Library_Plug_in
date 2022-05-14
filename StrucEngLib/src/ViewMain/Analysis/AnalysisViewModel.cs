using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Rhino;

namespace StrucEngLib.Analysis
{
    /// <summary>View Model for Compas Analysis Settings </summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;

        public ObservableCollection<AnalysisItemViewModel> AnalysisViewItems { get; }

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

        public AnalysisViewModel(MainViewModel vm)
        {
            _vm = vm;
            AnalysisViewItems = new ObservableCollection<AnalysisItemViewModel>();
            AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 1"});
            AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 2"});
            AnalysisViewItems.Add(new AnalysisItemViewModel() {StepName = "Step 3"});
        }
    }
}