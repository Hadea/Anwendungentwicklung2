namespace AddressBookLogic
{
    public class AddContactCommand : BaseCommand
    {
        public AddContactCommand(AddressBookViewModel parent) : base(parent) {}

        public override void Execute(object parameter)
        {
            ContactViewModel newContact = new ContactViewModel();
            _parent.logic.ContactList.Add(newContact);
            _parent.Contacts.Add(newContact);
            _parent.SelectedContact = newContact;
        }
    }
}