using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AddressBookLogic
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _firstName;
        private string _lastName;
        private string _street;
        private string _houseNo;
        private string _zip;
        private string _city;
        private string _state;
        private string _country;

        public ContactViewModel()
        {
            WebProfiles = new ObservableCollection<string>();
        }

        public ContactViewModel(string FirstName, string LastName, string Street, string HouseNo, string ZIP,string City, string State, string Country)
        {
            _firstName = FirstName;
            _lastName = LastName;
            _street = Street;
            _houseNo = HouseNo;
            _zip = ZIP;
            _city = City;
            _state = State;
            _country = Country;
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                if (_street != value)
                {
                    _street = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
                }
            }
        }

        public string HouseNo
        {
            get { return _houseNo; }
            set
            {
                if (_houseNo != value)
                {
                    _houseNo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HouseNo)));
                }
            }
        }

        public string ZIP
        {
            get { return _zip; }
            set
            {
                if (_zip != value)
                {
                    _zip = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZIP)));
                }
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
                }
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
                }
            }
        }

        public ObservableCollection<string> WebProfiles { get; set; }
    }
}