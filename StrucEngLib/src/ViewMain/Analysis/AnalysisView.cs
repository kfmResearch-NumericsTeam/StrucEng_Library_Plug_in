using Eto.Drawing;
using Eto.Forms;

namespace StrucEngLib.Analysis
{
    /// <summary></summary>
    public class AnalysisView: DynamicLayout
    {
        private readonly AnalysisViewModel _vm;

        public AnalysisView(AnalysisViewModel vm)
        {
            _vm = vm;
            
            // AddRow(UiUtils.GenerateTitle("Step 5: Define Analysis Steps"));
            AddRow(
                new GroupBox
                {
                    Text = "",
                    Padding = new Padding(5),
                    Content = new TableLayout
                    {
                        Spacing = new Size(10, 10),
                        Rows =
                        {
                            
                        }
                    }
                });
        }
    }
}