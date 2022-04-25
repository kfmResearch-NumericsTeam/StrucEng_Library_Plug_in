using System;
using System.Collections.Generic;
using CodeGenerator.Views;
using Eto.Drawing;
using Eto.Forms;

namespace CodeGenerator
{
    public interface IControl
    {
        void Enable(bool state);
    }
    public class SelectionViewModel: ViewModelBase
    {

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                Rhino.RhinoApp.WriteLine("selectidnex changed, {0}", value);
                _selectedIndex = value;
                OnPropertyChanged();
                View = (Control) _views[_selectedIndex];
            }
        }

        private Control _view;
        private List<Control> _views;
        
        public  SelectionViewModel(List<Control> views)
        {
            _views = views;
        }

        public Control View
        {
            get => _view;
            set
            {
                _view = value;
                OnPropertyChanged();
                Rhino.RhinoApp.WriteLine("view changed");
            }
        }
    }
    
    public class SelectionView: DynamicLayout
    {
        private readonly string _sectionLabel;
        private readonly List<string> _items;
        private readonly SelectionViewModel _vm;

        private DropDown _dbSectionSelection;
        private DynamicLayout _propertyLayout;

        public SelectionView(string sectionLabel, List<string> items, List<Control> views)
        {
            _sectionLabel = sectionLabel;
            _items = items;
            if (items.Count != views.Count)
            {
                throw new Exception("items and views must have same count");
            }
            _vm = new SelectionViewModel(views);
            BuildGui(views);
        }

        protected void BuildGui(List<Control> views)
        {
            Padding = new Padding(5);
            Spacing = new Size(5, 5);


            var sectionLayout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            Add(sectionLayout);

            sectionLayout.Rows.Add(new ViewSeparator() {Text = _sectionLabel});
            sectionLayout.Rows.Add(TableLayout.HorizontalScaled((_dbSectionSelection = new DropDown { })));
            
            _dbSectionSelection.DataStore = _items;
            _dbSectionSelection.Bind<int>("SelectedIndex", _vm, "SelectedIndex",
                DualBindingMode.TwoWay);
            
            _propertyLayout = new DynamicLayout
            {
                Padding = new Padding(5) { },
                Spacing = new Size(5, 5),
                Visible = true
            };
            _propertyLayout.Add(views[0]);
            _vm.View = views[0];
            _vm.SelectedIndex = 0;
            
            _propertyLayout.Bind<Control>("Content", _vm, "View",
                DualBindingMode.TwoWay);
            
            sectionLayout.Rows.Add(_propertyLayout);
        }
    }
}