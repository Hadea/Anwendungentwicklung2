using System;
using System.Windows.Input;

namespace ChatClientGUI
{
    class GenericParameterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _executeAction;
        private readonly Func<object,bool> _canExecuteAction;

        public bool CanExecute(object parameter)//TODO: pass parameter
        {
            if (_canExecuteAction == null) return true;
            return _canExecuteAction(parameter);
        }

        public GenericParameterCommand(Action<object> Execute, Func<object,bool> CanExecute = null)
        {
            _executeAction = Execute;
            if (CanExecute != null)
                _canExecuteAction = CanExecute;
        }
        public void Execute(object parameter) => _executeAction?.Invoke(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
