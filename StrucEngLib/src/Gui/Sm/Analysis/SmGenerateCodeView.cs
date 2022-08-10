using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Gui;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for Analysis control in sandwich model
    /// </summary>
    public class SmGenerateCodeView : CommonGenerateCodeView
    {
        public SmGenerateCodeView(CommonGenerateCodeViewModel vm) : base(vm, "Generate Code for Sandwich Model")
        {
        }
    }
}