using Eto.Forms;
using StrucEngLib.Model;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.LinFe.Layer
{
    /// <summary>View to show set specific actions</summary>
    public class SetView : DynamicLayout
    {
        public SetView(NewSectionViewModel<SetDisplacementType> vm)
        {
            Add(new NewSelectionView<SetDisplacementType>("Displacements", vm));
        }
    }
}