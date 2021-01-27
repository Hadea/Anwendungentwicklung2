namespace AddressBookLogic
{
    public class DeleteLinkCommand : BaseCommand
    {
        public DeleteLinkCommand(AddressBookViewModel parent) : base(parent) {}

        public override void Execute(object parameter)
        {
            _parent.SelectedContact.WebProfiles.Remove(parameter as WebLinkViewModel);
        }
    }
}