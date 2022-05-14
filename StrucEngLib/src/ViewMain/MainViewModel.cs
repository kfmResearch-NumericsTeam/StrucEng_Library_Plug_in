using Eto.Forms;
using Rhino.UI;
using StrucEngLib.Analysis;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ListLayerViewModel ListLayerVm { get; set; }
        public LayerDetailsViewModel DetailLayerVm { get; set; }
        public ListLoadViewModel ListLoadVm { get; set; }
        public ListStepViewModel ListStepVm { get; set; }
        public ErrorViewModel ErrorVm { get; set; }
        public AnalysisViewModel AnalysisVm { get; set; }
        
        public MainViewModel()
        {
            ListLayerVm = new ListLayerViewModel(this);
            DetailLayerVm = new LayerDetailsViewModel(this);
            ListLoadVm = new ListLoadViewModel(this);
            ListStepVm = new ListStepViewModel(this);
            ErrorVm = new ErrorViewModel();
            AnalysisVm = new AnalysisViewModel(this);
        }
    }
}