using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NewGarbageAndPeople.VM.VMTools
{
    public partial class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Signal([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
