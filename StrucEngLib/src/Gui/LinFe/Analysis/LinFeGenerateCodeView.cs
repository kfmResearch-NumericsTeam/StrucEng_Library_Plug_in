using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View for Analysis control in sandwich model
    /// </summary>
    public class LinFeGenerateCodeView : CommonGenerateCodeView
    {
        public LinFeGenerateCodeView(LinFeGenerateCodeViewModel vm) : base(vm, "Generate Code for LinFE Model")
        {
        }
    }
}