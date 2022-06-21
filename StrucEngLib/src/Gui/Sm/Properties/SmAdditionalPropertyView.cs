using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;

namespace StrucEngLib.Sm
{
    /// <summary>
    /// View model for a single additional property in sandwich model
    /// </summary>
    public class SmAdditionalPropertyView : DynamicLayout
    {
        private Button _btShowImage;

        /// <summary>
        /// XXX: Requires DataContext to be set according to selection to change content of properties
        /// E.g._view.Bind<object>(nameof(_view.DataContext), _vm, nameof(_vm.SelectedProperty));
        /// </summary>
        public SmAdditionalPropertyView()
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 5);

            UiUtils.AddLabelTextRow(this, "d_strich_bot",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichBot), "40");
            UiUtils.AddLabelTextRow(this, "d_strich_top",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichTop), "40");
            UiUtils.AddLabelTextRow(this, "fc_k",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcK), "20");
            UiUtils.AddLabelTextRow(this, "fc_theta_grad_kern",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcThetaGradKern), "45");
            UiUtils.AddLabelTextRow(this, "fs_d",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FsD), "435");
            UiUtils.AddLabelTextRow(this, "alpha_bot",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaBot), "0");
            UiUtils.AddLabelTextRow(this, "beta_bot",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaBot), "90");
            UiUtils.AddLabelTextRow(this, "alpha_top",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaTop), "0");
            UiUtils.AddLabelTextRow(this, "beta_top",
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaTop), "90");

            AddRow(null);
            AddRow(TableLayout.AutoSized((_btShowImage = new Button
            {
                Size = new Size(110, -1),
                Text = "Show Image...",
            })));

            _btShowImage.Click += (sender, args) =>
            {
                var d = new ShowImageForm()
                {
                    Owner = RhinoEtoApp.MainWindow
                };
                d.Show();
            };
        }
    }
}