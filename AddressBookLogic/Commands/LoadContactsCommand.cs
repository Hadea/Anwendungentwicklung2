namespace AddressBookLogic
{
    class LoadContactsCommand :BaseCommand
    {
        public LoadContactsCommand(AddressBookViewModel parent) : base(parent) { }

        public override void Execute(object parameter)
        {
            _parent.logic.Load();
            _parent.Contacts = new(_parent.logic.ContactList);
        }
    }
}
