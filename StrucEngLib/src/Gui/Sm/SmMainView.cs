using System.Linq;
using Eto.Forms;
using Rhino.UI.Controls;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui.Sm
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
            var layout = new DynamicLayout();
            var holder = new EtoCollapsibleSectionHolder();
            layout.AddRow(holder);
            ScrollHelper.ScrollParent(holder);
            new[]
            {
                new CollapsibleSectionHolder("About Sandwich Model",
                    new AboutSmView()),
                
                new CollapsibleSectionHolder("Step 1: Define Materials and Constraints",
                new SmAdditionalPropertiesView(_vm.SmSettingVm)),

                new CollapsibleSectionHolder("Step 2: Run Analysis",
                    new SmAnalysisView(_vm.AnalysisVm),
                    new SmGenerateCodeView(_vm.GenerateCodeVm)
                    )
            }.ToList().ForEach(e => holder.Add(e));
            Content = layout;
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}