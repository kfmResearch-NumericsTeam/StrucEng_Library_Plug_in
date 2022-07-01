using System.Diagnostics;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using StrucEngLib.Layer;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// Main View for Sandwich Model
    /// </summary>
    public class SmMainView : Scrollable
    {
        private SmMainViewModel _vm;
        public static SmMainView Instance { get; private set; }

        public SmMainView(SmMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(SmMainViewModel vm)
        {
            _vm = vm;
            BuildUi();
        }

        private void BuildUi()
        {
            BackgroundColor = new Label().BackgroundColor;
            Content = new DynamicLayout()
            {
                Padding = new Padding(10, 10),
                Spacing = new Size(0, 10),
                Rows =
                {
                    UiUtils.GenerateTitle("Step 1: Define Materials and Constraints"),
                    new SmAdditionalPropertiesView(_vm.SmSettingVm),
                    // UiUtils.GenerateTitle("Step 2: Define Model Settings"),
                    // new SmSettingView(_vm.SmSettingVm),
                    UiUtils.GenerateTitle("Step 2: Run Analysis"),
                    new SmAnalysisView(_vm.AnalysisVm),
                    new SmGenerateCodeView(_vm.GenerateCodeVm),
                    new Label()
                }
            };
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}