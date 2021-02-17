using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChatClientGUI
{
    public class RoomViewModel : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public ObservableCollection<UserViewModel> UserList { get;} = new();
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
