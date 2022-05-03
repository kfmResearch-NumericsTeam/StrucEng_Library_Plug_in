using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using StrucEngLib.Views;

namespace StrucEngLib
{
    class SelectionViewModel : ViewModelBase
    {
        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                View = _views[_selectedIndex];
            }
        }

        private Control _view;
        private List<Control> _views;

        public SelectionViewModel(List<Control> views)
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
            }
        }
    }

    /// <summary>
    /// View to show a dropdown menu to switch to different views according to selection in dropdown menu
    /// </summary>
    public class SelectionView : DynamicLayout
    {
        private readonly string _sectionLabel;
        private readonly List<string> _items;
        private readonly SelectionViewModel _vm;

        private DropDown _dbSectionSelection;
        private DynamicLayout _propertyLayout;

        /// <param name="sectionLabel"></param> Titel to show
        /// <param name="items"></param> List of labels to show in dropdown
        /// <param name="views"></param> List of views to show when selected in dropdown
        /// <exception cref="Exception"></exception>
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
                Padding = new Padding {Top = 5, Left = 10, Bottom = 0, Right = 0},
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