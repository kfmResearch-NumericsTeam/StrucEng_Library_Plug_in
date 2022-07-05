using Eto.Drawing;
using Eto.Forms;
using Rhino.UI;
using StrucEngLib.Views;

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
            Spacing = new Size(5, 1);

            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "d_strich",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichTop), "40");
            UiUtils.AddLabelTextRow(
                this,
                new SubscriptLabel()
                {
                    Text = "d_strich",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichBot), "40");
            UiUtils.AddLabelTextRow(this, 
                new SubscriptLabel()
                {
                    Text = "alpha",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaTop), "0");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "alpha",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaBot), "0");
            
            UiUtils.AddLabelTextRow(this, 
                new SubscriptLabel()
                {
                    Text = "beta",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaTop), "90");
            UiUtils.AddLabelTextRow(this, 
                new SubscriptLabel()
                {
                    Text = "beta",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaBot), "90");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "fc",
                    TextSubscript = "k"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcK), "20");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "fc",
                    TextSubscript = "theta_grad_kern"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcThetaGradKern), "45");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "fc",
                    TextSubscript = "d"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FsD), "435");

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