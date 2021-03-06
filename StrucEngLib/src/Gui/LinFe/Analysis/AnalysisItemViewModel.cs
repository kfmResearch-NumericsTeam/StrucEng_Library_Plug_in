using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib.Analysis
{
    /// <summary>Vm for a single analysis item</summary>
    public class AnalysisItemViewModel : ViewModelBase
    {
        public AnalysisSetting Model => StepModel.Setting;

        public Model.Step StepModel { get; }

        public AnalysisItemViewModel(Model.Step model)
        {
            StepModel = model;
            init();
            if (model.Setting == null)
            {
                model.Setting = new AnalysisSetting();
            }

            ModelToVm(model.Setting, this);
        }

        public override void UpdateModel()
        {
            VmToModel(this, Model);
        }

        public static void ModelToVm(AnalysisSetting model, AnalysisItemViewModel v)
        {
            v.StepName = model.StepId;
            v.Include = model.Include;
            v.Rf = model.Rf;
            v.Rm = model.Rm;
            v.U = model.U;
            v.Ur = model.Ur;
            v.Cf = model.Cf;
            v.Cm = model.Cm;

            v.ShellMoments = model.ShellMoments;
            v.SectionMoments = model.SectionMoments;
            v.ShellForces = model.ShellForces;
            v.SectionForces = model.SectionForces;
            v.SpringForces = model.SpringForces;
        }

        public static void VmToModel(AnalysisItemViewModel v, AnalysisSetting model)
        {
            model.StepId = v.StepName;
            model.Include = v.Include ?? false;
            model.Rf = v.Rf ?? false;
            model.Rm = v.Rm ?? false;
            model.U = v.U ?? false;
            model.Ur = v.Ur ?? false;
            model.Cf = v.Cf ?? false;
            model.Cm = v.Cm ?? false;

            model.ShellMoments = v.ShellMoments ?? false;
            model.SectionMoments = v.SectionMoments ?? false;
            model.ShellForces = v.ShellForces ?? false;
            model.SectionForces = v.SectionForces ?? false;
            model.SpringForces = v.SpringForces ?? false;
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

            _strainDerived = false;
            _strainShells = false;
            _strainBeams = false;
            _stressDerived = false;
            _stressShells = false;
            _stressBeams = false;
            _shellCurvatures = false;
            _sectionCurvatures = false;
            _sectionStrains = false;
            _shellMoments = false;
            _sectionMoments = false;
            _shellForces = false;
            _sectionForces = false;
            _springForces = false;
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

        private bool _strainDerived;

        public bool? StrainDerived
        {
            get => _strainDerived;
            set
            {
                _strainDerived = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _strainShells;

        public bool? StrainShells
        {
            get => _strainShells;
            set
            {
                _strainShells = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _strainBeams;

        public bool? StrainBeams
        {
            get => _strainBeams;
            set
            {
                _strainBeams = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _stressDerived;

        public bool? StressDerived
        {
            get => _stressDerived;
            set
            {
                _stressDerived = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _stressShells;

        public bool? StressShells
        {
            get => _stressShells;
            set
            {
                _stressShells = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _stressBeams;

        public bool? StressBeams
        {
            get => _stressBeams;
            set
            {
                _stressBeams = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _shellCurvatures;

        public bool? ShellCurvatures
        {
            get => _shellCurvatures;
            set
            {
                _shellCurvatures = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _sectionCurvatures;

        public bool? SectionCurvatures
        {
            get => _sectionCurvatures;
            set
            {
                _sectionCurvatures = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _sectionStrains;

        public bool? SectionStrains
        {
            get => _sectionStrains;
            set
            {
                _sectionStrains = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _shellMoments;

        public bool? ShellMoments
        {
            get => _shellMoments;
            set
            {
                _shellMoments = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _sectionMoments;

        public bool? SectionMoments
        {
            get => _sectionMoments;
            set
            {
                _sectionMoments = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _shellForces;

        public bool? ShellForces
        {
            get => _shellForces;
            set
            {
                _shellForces = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _sectionForces;

        public bool? SectionForces
        {
            get => _sectionForces;
            set
            {
                _sectionForces = value ?? false;
                OnPropertyChanged();
            }
        }

        private bool _springForces;

        public bool? SpringForces
        {
            get => _springForces;
            set
            {
                _springForces = value ?? false;
                OnPropertyChanged();
            }
        }
    }
}