using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI.Controls;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.LocalCoordinate;
using StrucEngLib.Step;
using StrucEngLib.Utils;

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
            var holder = new EtoCollapsibleSectionHolder();
            ScrollHelper.ScrollParent(holder);
            new[]
            {
                new CollapsibleSectionHolder("About Sandwich Model",
                    new AboutSmView()),
                
                new CollapsibleSectionHolder("Step 1: Define Materials and Constraints",
                    new SmAdditionalPropertiesView(_vm.SmSettingVm)),

                new CollapsibleSectionHolder("Step 2: Run Analysis",
                    new SmAnalysisView(_vm.AnalysisVm),
                    new SmGenerateCodeView(_vm.GenerateCodeVm),
                    new Label())
            }.ToList().ForEach(e => holder.Add(e));
            Content = holder;
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}