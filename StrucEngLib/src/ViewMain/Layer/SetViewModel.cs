using System.Collections.Generic;
using Eto.Forms;
using Rhino;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary>View model for set data</summary>
    public class SetViewModel : NewSectionViewModel<SetDisplacementType>
    {
        private readonly MainViewModel _vm;

        public SetViewModel(MainViewModel vm) : base(
            new List<ListItem>
            {
                new ListItem()
                {
                    Key = SetDisplacementType.GENERAL.ToString(),
                    Text = "General Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.PINNED.ToString(),
                    Text = "Pinned Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.FIXED.ToString(),
                    Text = "Fixed Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.FIXED_XX.ToString(),
                    Text = "Fixed XX Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.FIXED_YY.ToString(),
                    Text = "Fixed YY Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.FIXED_ZZ.ToString(),
                    Text = "Fixed ZZ Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_X.ToString(),
                    Text = "Roller X Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_Y.ToString(),
                    Text = "Roller Y Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_Z.ToString(),
                    Text = "Roller Z Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_XY.ToString(),
                    Text = "Roller XY Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_YZ.ToString(),
                    Text = "Roller YZ Displacement"
                },
                new ListItem()
                {
                    Key = SetDisplacementType.ROLLER_XZ.ToString(),
                    Text = "Roller XZ Displacement"
                }
            })
        {
            _vm = vm;
            var set = (Set) _vm.ListLayerVm.SelectedLayer;
            EntryName = set.SetDisplacementType;
        }

        protected override void OnEntryChanged(SetDisplacementType old, SetDisplacementType entryName)
        {
            var set = (Set) _vm.ListLayerVm.SelectedLayer;
            if (entryName == SetDisplacementType.GENERAL)
            {
                set.SetDisplacementType = entryName;
                EntryView = new SetGeneralDisplacementView(new SetGeneralDisplacementViewModel(_vm.ListLayerVm));
            }
            else
            {
                set.SetDisplacementType = entryName;
                // XXX: We clear general displacement if other displacement was selected
                set.SetGeneralDisplacement = null;
                EntryView = new DynamicLayout();
            }
        }
    }
}