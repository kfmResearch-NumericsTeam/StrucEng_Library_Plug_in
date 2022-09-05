using Microsoft.SqlServer.Server;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib.Gui.LinFe.Load
{
    /// <summary>Point load vm</summary>
    public class PointLoadViewModel : AbstractLoadViewModel
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
        
        private string _xx;

        public string XX
        {
            get => _xx;
            set
            {
                _xx = value;
                OnPropertyChanged();
            }
        }
        
        private string _yy;

        public string YY
        {
            get => _yy;
            set
            {
                _yy = value;
                OnPropertyChanged();
            }
        }
        
        private string _zz;

        public string ZZ
        {
            get => _zz;
            set
            {
                _zz = value;
                OnPropertyChanged();
            }
        }

        // XXX: This load currently has no properties
        public PointLoadViewModel(LinFeMainViewModel mainVm) : base(mainVm)
        {
        }

        protected override void StoreVmToModel()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Point)
            {
                var g = (LoadPoint) ListLoadVm.SelectedLoad;
                g.Z = Z;
                g.X = X;
                g.Y = Y;
                g.ZZ = ZZ;
                g.XX = XX;
                g.YY = YY;
            }
        }

        protected override void StoreModelToVm()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Point)
            {
                var l = (LoadPoint) ListLoadVm.SelectedLoad;
                _z = l.Z;
                _x = l.X;
                _y = l.Y;
                _xx = l.XX;
                _zz = l.ZZ;
                _yy = l.YY;
            }
        }
    }
}