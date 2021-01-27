using System.Windows.Input;

namespace AddressBookLogic
{
    public class NavigateWebCommand : BaseCommand
    {
        public NavigateWebCommand(AddressBookViewModel parent) : base(parent) { }

        public override void Execute(object parameter)
        {
            _parent.WebAddress = parameter as string;
        }
    }
}