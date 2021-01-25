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
            Contacts = new();
        }
        public ICommand Command_AddContact { get; init; }
        public ICommand Command_DeleteContact { get; init; }
        public ICommand Command_LoadContacts { get; init; }
        public ICommand Command_SaveContacts { get; init; }
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
                    (Command_DeleteContact as DeleteContactCommand ).RaiseCanExecuteChanged();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedContact)));
                }
            }
        }
        private ContactViewModel _selectedContact;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ContentFilter { get; set; }

    }
}
