using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.UI;
using StrucEngLib.Analysis;
using StrucEngLib.Layer;
using StrucEngLib.Load;
using StrucEngLib.LocalCoordinate;
using StrucEngLib.Sm;
using StrucEngLib.Step;

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
            layout.Padding = new Padding(10, 10);
            layout.Spacing = new Size(0, 10);
            
            layout.AddRow(UiUtils.GenerateTitle("Step 1: Define Parts, Materials and Constraints"));
            layout.AddRow(new ListLayerView(vm));
            layout.AddRow(UiUtils.GenerateTitle("Step 2: Define Local Coordinates"));
            layout.AddRow(new LoadConstraintView(new LoadConstraintViewModel(vm)));
            layout.AddRow(UiUtils.GenerateTitle("Step 3: Define Loads"));
            layout.AddRow(new ListLoadView(vm.ListLoadVm));
            layout.AddRow(UiUtils.GenerateTitle("Step 4: Define Analysis Steps"));
            layout.AddRow(new ListStepView(vm.ListStepVm));
            layout.AddRow(UiUtils.GenerateTitle("Step 5: Run Analysis"));
            layout.AddRow(new AnalysisView(vm.AnalysisVm));
            layout.AddRow(new LinFeGenerateCodeView(vm.GenerateCodeVm));
            Content = layout;
        }

        public void DisposeUi()
        {
            Content.Dispose();
            Content = null;
        }
    }
}