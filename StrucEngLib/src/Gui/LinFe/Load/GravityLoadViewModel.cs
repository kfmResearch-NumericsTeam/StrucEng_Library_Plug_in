using Microsoft.SqlServer.Server;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib.Gui.LinFe.Load
{
    /// <summary>Gravity load vm</summary>
    public class GravityLoadViewModel : AbstractLoadViewModel
    {
        private string _x;

        public string X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        public string _y;

        public string Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        public string _z;

        public string Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged();
            }
        }

        // XXX: This load currently has no properties
        public GravityLoadViewModel(LinFeMainViewModel mainVm) : base(mainVm)
        {
        }

        protected override void StoreVmToModel()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Gravity)
            {
                var g = (LoadGravity) ListLoadVm.SelectedLoad;
                g.Z = Z;
                g.X = X;
                g.Y = Y;
            }
        }

        protected override void StoreModelToVm()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Gravity)
            {
                var l = (LoadGravity) ListLoadVm.SelectedLoad;
                _z = l.Z;
                _x = l.X;
                _y = l.Y;
            }
        }
    }
}