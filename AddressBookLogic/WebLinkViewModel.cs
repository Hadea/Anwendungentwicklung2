using System.ComponentModel;

namespace AddressBookLogic
{
    public class WebLinkViewModel : INotifyPropertyChanged
    {
        public string Link
        {
            get { return _link; }
            set
            {
                if (_link != value)
                {
                    _link = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Link)));
                }
            }
        }
        private string _link = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
