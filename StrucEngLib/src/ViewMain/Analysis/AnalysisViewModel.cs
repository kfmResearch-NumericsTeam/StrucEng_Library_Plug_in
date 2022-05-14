using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Rhino;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisViewModel : ViewModelBase
    {
        private readonly MainViewModel _vm;

        public ObservableCollection<AnalysisItemViewModel> AnalysisViewItems { get; }

        private AnalysisItemViewModel _selectedItem;

        public bool SelectedItemVisible
        {
            get => SelectedItem != null;
        }

        public AnalysisItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    RhinoApp.WriteLine("{0}", _selectedItem.StepName);
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

        public void CellDoubleClick(object sender, GridCellMouseEventArgs e)
        {
            if (e != null && e.Row >= 0 && e.Row < AnalysisViewItems.Count)
            {
                var i = AnalysisViewItems[e.Row];
                RhinoApp.WriteLine("Double clicked {0}", i.StepName);
                i.Include = !i.Include;
            }
        }
    }
}