using Eto.Forms;
using Rhino.UI;
using Rhino.UI.Controls;

namespace StrucEngLib.Utils
{
    /// <summary>A holder to show content in a collapsible section</summary>
    public class CollapsibleSectionHolder : EtoCollapsibleSection
    {
        private LocalizeStringPair _label;
        private DynamicLayout _layout;

        public bool ExpandedByDefault { get; set; } = false;

        public override bool InitiallyExpanded
        {
            get => ExpandedByDefault;
        }

        public CollapsibleSectionHolder(string label, params Control[] controls)
        {
            _label = new LocalizeStringPair(label, label);
            _layout = new DynamicLayout();

            foreach (var control in controls)
            {
                ScrollHelper.ScrollParent(control);
                _layout.AddRow(control);
            }

            Content = _layout;
            ScrollHelper.ScrollParent(this);
        }

        public DynamicLayout Layout => _layout;

        public override LocalizeStringPair Caption
        {
            get => _label;
        }

        public override int SectionHeight
        {
            /*
             * Set this to 0 otherwise rhino will unfold
             * all sections if we dynamically change the size of a section
             */
            get => 0;
        }
    }
}