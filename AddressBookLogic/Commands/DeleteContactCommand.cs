namespace AddressBookLogic
{
    internal class DeleteContactCommand : BaseCommand
    {
        public DeleteContactCommand(AddressBookViewModel parent) : base(parent){}

        public override bool CanExecute(object parameter)
        {
            return _parent.SelectedContact != null;
        }

        public override void Execute(object parameter)
        {
            _parent.Contacts.Remove(_parent.SelectedContact);
            _parent.SelectedContact = null;
        }

    }
}