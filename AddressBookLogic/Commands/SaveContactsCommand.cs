using System.Linq;

namespace AddressBookLogic
{
    class SaveContactsCommand : BaseCommand
    {
        public SaveContactsCommand(AddressBookViewModel parent) : base(parent) { }

        public override void Execute(object parameter)
        {
            DataStorage.Save(_parent.Contacts.ToList());
        }
    }
}
