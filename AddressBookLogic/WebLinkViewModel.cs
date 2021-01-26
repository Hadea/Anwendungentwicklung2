using System.ComponentModel;

namespace AddressBookLogic
{
    public class WebLinkViewModel : INotifyPropertyChanged
    {

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }
        private string _description = string.Empty;

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
