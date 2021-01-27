using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AddressBookLogic
{
    public class AddressBookViewModel : INotifyPropertyChanged
    {
        public AddressBookViewModel()
        {
            Command_AddContact = new AddContactCommand(this);
            Command_DeleteContact = new DeleteContactCommand(this);
            Command_LoadContacts = new LoadContactsCommand(this);
            Command_SaveContacts = new SaveContactsCommand(this);
            Command_NavigateWeb = new NavigateWebCommand(this);
            Command_AddLink = new AddLinkCommand(this);
            Command_DeleteLink = new DeleteLinkCommand(this);

            Contacts = new();
        }
        public ICommand Command_AddContact { get; init; }
        public ICommand Command_DeleteContact { get; init; }
        public ICommand Command_LoadContacts { get; init; }
        public ICommand Command_SaveContacts { get; init; }
        public ICommand Command_NavigateWeb { get; init; }
        public ICommand Command_AddLink { get; init; }
        public ICommand Command_DeleteLink { get; init; }
        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contacts)));
                }
            }
        }
        private ObservableCollection<ContactViewModel> _contacts;

        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (_selectedContact != value)
                {
                    _selectedContact = value;
                    (Command_DeleteContact as DeleteContactCommand).RaiseCanExecuteChanged();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedContact)));
                }
            }
        }
        private ContactViewModel _selectedContact;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ContentFilter
        {
            get => _contentFilter;
            set
            {
                if (_contentFilter != value)
                {
                    _contentFilter = value;
                    reloadContacts();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contacts)));
                }
            }
        }

        private string _webAddress;

        public string WebAddress
        {
            get => _webAddress; 
            set
            {
                _webAddress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WebAddress)));
            }
        }



        private void reloadContacts()
        {
            Contacts.Clear();
            foreach (var item in logic.ContactList)
                if (item.FirstName.Contains(_contentFilter) ||
                    item.LastName.Contains(_contentFilter) ||
                    item.HouseNo.Contains(_contentFilter) ||
                    item.City.Contains(_contentFilter) ||
                    item.State.Contains(_contentFilter) ||
                    item.Street.Contains(_contentFilter) ||
                    item.Country.Contains(_contentFilter))
                    Contacts.Add(item);
        }

        private string _contentFilter;
        public DataStorage logic = new();
    }
}
