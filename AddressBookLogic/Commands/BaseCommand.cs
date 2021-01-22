using System;
using System.Windows.Input;

namespace AddressBookLogic
{
    public abstract class BaseCommand : ICommand
    {
        protected readonly AddressBookViewModel _parent;
        public event EventHandler CanExecuteChanged;
        public abstract void Execute(object parameter);
        public BaseCommand(AddressBookViewModel parent) => _parent = parent;
        public virtual bool CanExecute(object parameter) {return true;}
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
