using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.UI;
using StrucEngLib.Model;
using Layer = StrucEngLib.Model.Layer;

namespace StrucEngLib.Gui.LinFe.Load
{
    /// <summary>
    /// Vm for Area load
    /// </summary>
    public class AreaLoadViewModel : AbstractLoadViewModel
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

        private string _y;
        public string Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        private String _z;
        public String Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged();
            }
        }

        private String _axes;

        public String Axes
        {
            get => _axes;
            set
            {
                _axes = value;
                OnPropertyChanged();
            }
        }


        public AreaLoadViewModel(LinFeMainViewModel mainVm) : base(mainVm)
        {
        }

        protected override void StoreVmToModel()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Area)
            {
                var l = (LoadArea) ListLoadVm.SelectedLoad;
                l.Axes = Axes;
                l.Z = Z;
                l.X = X;
                l.Y = Y;
            }
        }

        protected override void StoreModelToVm()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Area)
            {
                var l = (LoadArea) ListLoadVm.SelectedLoad;
                _z = l.Z;
                _x = l.X;
                _y = l.Y;
                Axes = l.Axes;
            }
        }
    }
}