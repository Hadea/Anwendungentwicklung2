using System;
using System.Windows.Input;

namespace ChatClientGUI
{
    class GenericCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteAction;

        public bool CanExecute(object parameter)
        {
            if (_canExecuteAction == null) return true;
            return _canExecuteAction.Invoke();
        }

        public GenericCommand(Action Execute, Func<bool> CanExecute = null)
        {
            _executeAction = Execute;
            if (CanExecute != null)
                _canExecuteAction = CanExecute;
        }
        public void Execute(object parameter) => _executeAction?.Invoke();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
