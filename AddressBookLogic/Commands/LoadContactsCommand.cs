namespace AddressBookLogic
{
    class LoadContactsCommand :BaseCommand
    {
        public LoadContactsCommand(AddressBookViewModel parent) : base(parent) { }

        public override void Execute(object parameter)
        {
            _parent.Contacts = new(DataStorage.Load());
        }
    }
}
