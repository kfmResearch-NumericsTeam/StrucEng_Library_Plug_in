using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using Rhino.UI.Controls;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.LocalCoordinate;
using StrucEngLib.Sm;
using StrucEngLib.Step;
using StrucEngLib.Utils;

namespace StrucEngLib
{
    /// <summary>
    /// Main view for lin fe
    /// </summary>
    public class LinFeMainView : Scrollable
    {
        public static LinFeMainView Instance { get; private set; }

        public LinFeMainView(LinFeMainViewModel vm)
        {
            Instance = this;
            LoadUi(vm);
        }

        public void LoadUi(LinFeMainViewModel vm)
        {
            BackgroundColor = new Label().BackgroundColor;
            Padding = new Padding();
            var layout = new DynamicLayout();
            var holder = new EtoCollapsibleSectionHolder();
            layout.AddRow(holder);
            ScrollHelper.ScrollParent(holder);
            new[]
            {
                new CollapsibleSectionHolder("About LinFE Model",
                    new AboutLinFeView()),

                new CollapsibleSectionHolder("Step 1: Define Parts, Materials and Constraints",
                    new ListLayerView(vm)),

                new CollapsibleSectionHolder("Step 2: Define Local Coordinates",
                    new LoadConstraintView(new LoadConstraintViewModel(vm))),

                new CollapsibleSectionHolder("Step 3: Define Loads",
                    new ListLoadView(vm.ListLoadVm)),

                new CollapsibleSectionHolder("Step 4: Define Analysis Steps",
                    new ListStepView(vm.ListStepVm)),

                new CollapsibleSectionHolder("Step 5: Run Analysis",
                    new AnalysisView(vm.AnalysisVm),
                    new LinFeGenerateCodeView(vm.GenerateCodeVm))
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