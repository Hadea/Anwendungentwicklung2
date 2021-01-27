namespace AddressBookLogic
{
    public class AddLinkCommand : BaseCommand
    {
        public AddLinkCommand(AddressBookViewModel parent) : base(parent) {}

        public override void Execute(object parameter)
        {
            _parent.SelectedContact.WebProfiles.Add(new WebLinkViewModel());
        }
    }
}