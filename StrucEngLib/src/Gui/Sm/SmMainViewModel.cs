using System;
using System.Collections.Generic;
using Rhino.Geometry.Collections;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.Model;
using StrucEngLib.Step;

namespace StrucEngLib
{
    /// <summary>
    /// Context class with references to all primary view model of Sandwich Model
    /// </summary>
    public class SmMainViewModel : ViewModelBase
    {
        public Workbench Workbench { get; }

        public ErrorViewModel ErrorVm { get; }

        public SmMainViewModel(Workbench wb, ErrorViewModel error)
        {
            ErrorVm = error;
            Workbench = wb;
        }

        public override void UpdateModel()
        {
            new List<ViewModelBase>()
            {
            }.ForEach(vm => vm.UpdateModel());
        }

        public Workbench BuildModel()
        {
            try
            {
                UpdateModel();
            }
            catch (Exception e)
            {
                ErrorVm.ShowException("Error occured while extracting model data from view models", e);
            }

            return Workbench;
        }
    }
}