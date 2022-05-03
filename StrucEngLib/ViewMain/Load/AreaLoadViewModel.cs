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

namespace StrucEngLib
{
    /// <summary>
    /// Vm for Area load
    /// </summary>
    public class AreaLoadViewModel : AbstractLoadViewModel
    {

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

        public AreaLoadViewModel(MainViewModel mainVm) : base(mainVm)
        {
        }

        protected override void StoreVmToModel()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Area)
            {
                var l = (LoadArea) ListLoadVm.SelectedLoad;
                l.Axes = Axes;
                l.Z = Z;
            }
        }

        protected override void StoreModelToVm()
        {
            if (ListLoadVm.SelectedLoad.LoadType == LoadType.Area)
            {
                var l = (LoadArea) ListLoadVm.SelectedLoad;
                Z = l.Z;
                Axes = l.Axes;
            }
        }
    }
}