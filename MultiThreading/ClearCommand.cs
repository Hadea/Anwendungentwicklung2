using System;
using System.Windows.Input;

namespace MultiThreading
{
    internal class ClearCommand : ICommand
    {
        private pgeThreadingProgressCancelV2 _parent;

        public ClearCommand(pgeThreadingProgressCancelV2 Parent) => _parent = Parent;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _parent.IsNotRunning;
        }

        public void Execute(object parameter)
        {
            _parent.Clear();
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    }
}