using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StrucEngLib
{
    /// <summary>
    /// Base class for all view model classes
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
        }
        
        public virtual void UpdateModel()
        {
        }
        
        public virtual void UpdateViewModel()
        {
        }
    }
}