using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using CodeGenerator.Model;

namespace CodeGenerator.Step
{
    /**
     * View Model to select ordering of steps for Load processing.
     */
    public class StepViewModel : ViewModelBase
    {
        private readonly ListLayerViewModel _listLayerVm;
        private readonly ListLoadViewModel _listLoadVm;

        private ObservableCollection<string> _loadNames;

        public ObservableCollection<string> LoadNames
        {
            get => _loadNames;
            set
            {
                _loadNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Model.Step> Steps;

        public StepViewModel(ListLayerViewModel listLayerVm, ListLoadViewModel listLoadVm)
        {
            _listLayerVm = listLayerVm;
            _listLoadVm = listLoadVm;

            _listLoadVm.PropertyChanged += ListLoadVmOnPropertyChanged;
        }

        private void ListLoadVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listLoadVm.Loads))
            {
                LoadNames.Clear();
                foreach (var load in _listLoadVm.Loads)
                {
                    StringBuilder b = new StringBuilder();
                    b.Append(load.GetType().GetName());
                    b.Append(" (");
                    foreach (var l in load.Layers)
                    {
                        b.Append(l.GetName() + ";");
                    }

                    b.Append(")");
                    LoadNames.Add(b.ToString());
                }
            }
        }
    }
}