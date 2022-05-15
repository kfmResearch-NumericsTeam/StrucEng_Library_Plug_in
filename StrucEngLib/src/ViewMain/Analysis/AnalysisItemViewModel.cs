using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisItemViewModel : ViewModelBase
    {
        public AnalysisSetting Model { get; }

        public AnalysisItemViewModel(AnalysisSetting model)
        {
            Model = model;
            init();
            ModelToVm(model, this);
        }

        public override void UpdateModel()
        {
            VmToModel(this, Model);
        }

        public static void ModelToVm(AnalysisSetting model, AnalysisItemViewModel v)
        {
            v.StepName = model.StepId;
            v.Rf = model.Rf;
            v.Rm = model.Rm;
            v.U = model.U;
            v.Ur = model.Ur;
            v.Cf = model.Cf;
            v.Cm = model.Cm;
        }

        public static void VmToModel(AnalysisItemViewModel v, AnalysisSetting model)
        {
            model.StepId = v.StepName;
            model.Rf = v.Rf ?? false;
            model.Rm = v.Rm ?? false;
            model.U = v.U ?? false;
            model.Ur = v.Ur ?? false;
            model.Cf = v.Cf ?? false;
            model.Cm = v.Cm ?? false;
        }

        public void init()
        {
            _include = false;
            _rf = false;
            _rm = false;
            _u = false;
            _ur = false;
            _cf = false;
            _cm = false;
        }

        private string _stepName;

        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                OnPropertyChanged();
            }
        }

        private bool? _include = false;

        public bool? Include
        {
            get => _include;
            set
            {
                RhinoApp.WriteLine("Include set: {0}", value);
                _include = value;
                OnPropertyChanged();
            }
        }

        private bool? _rf = false;

        public bool? Rf
        {
            get => _rf;
            set
            {
                RhinoApp.WriteLine("rf set: {0}", value);
                _rf = value;
                OnPropertyChanged();
            }
        }


        private bool? _rm = false;

        public bool? Rm
        {
            get => _rm;
            set
            {
                _rm = value;
                OnPropertyChanged();
            }
        }


        private bool? _u = false;

        public bool? U
        {
            get => _u;
            set
            {
                _u = value;
                OnPropertyChanged();
            }
        }


        private bool? _ur = false;

        public bool? Ur
        {
            get => _ur;
            set
            {
                _ur = value;
                OnPropertyChanged();
            }
        }


        private bool? _cf = false;

        public bool? Cf
        {
            get => _cf;
            set
            {
                _cf = value;
                OnPropertyChanged();
            }
        }


        private bool? _cm = false;

        public bool? Cm
        {
            get => _cm;
            set
            {
                _cm = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"{nameof(_stepName)}: {_stepName}, {nameof(_include)}: {_include}," +
                   $" {nameof(_rf)}: {_rf}, {nameof(_rm)}: {_rm}, {nameof(_u)}: {_u}," +
                   $" {nameof(_ur)}: {_ur}, {nameof(_cf)}: {_cf}, {nameof(_cm)}: {_cm}";
        }
    }
}