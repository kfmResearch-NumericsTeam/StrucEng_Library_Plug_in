using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Gui.Sm;
using StrucEngLib.Utils;

namespace StrucEngLib.Gui
{
    /// <summary>
    /// View model for a single additional property in sandwich model
    /// </summary>
    public class SmAdditionalPropertyView : DynamicLayout
    {
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
                    Text = "d'",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichTop), "40");
            UiUtils.AddLabelTextRow(
                this,
                new SubscriptLabel()
                {
                    Text = "d'",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.DStrichBot), "40");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "α",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaTop), "0");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "α",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.AlphaBot), "0");

            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "β",
                    TextSubscript = "top"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaTop), "90");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "β",
                    TextSubscript = "bot"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.BetaBot), "90");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "f",
                    TextSubscript = "ck"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcK), "20");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "f",
                    TextSubscript = "c_θ_grad_kern"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FcThetaGradKern), "45");
            UiUtils.AddLabelTextRow(this,
                new SubscriptLabel()
                {
                    Text = "f",
                    TextSubscript = "sd"
                },
                Binding.Property<SmAdditionalPropertyViewModel, string>(m => m.FsD), "435");

            AddSeparateRow(null);
            AddSeparateRow(null);
        }
    }
}