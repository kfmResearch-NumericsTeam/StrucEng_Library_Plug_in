using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using StrucEngLib.Model;

namespace StrucEngLib
{
    /// <summary></summary>
    public abstract class NewSectionViewModel<ENUM_T> : ViewModelBase
    {
        public ObservableCollection<ListItem> EntryNames { get; }

        private Control _entryView;

        public Control EntryView
        {
            get => _entryView;
            set
            {
                _entryView = value;
                OnPropertyChanged();
                EntryViewVisible = _entryView != null;
            }
        }

        private bool _entryViewVisible = false;

        public bool EntryViewVisible
        {
            get => _entryViewVisible;
            set
            {
                _entryViewVisible = value;
                OnPropertyChanged();
            }
        }

        private ENUM_T _entryName;

        public ENUM_T EntryName
        {
            get { return _entryName; }
            set
            {
                ENUM_T old = _entryName;
                _entryName = value;
                OnPropertyChanged();
                OnEntryChanged(old, _entryName);
            }
        }

        protected abstract void OnEntryChanged(ENUM_T old, ENUM_T entryName);

        public NewSectionViewModel(List<ListItem> entries)
        {
            EntryNames = new ObservableCollection<ListItem>(entries);
        }
    }
}