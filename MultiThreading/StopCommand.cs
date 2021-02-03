using System;
using System.Windows.Input;

namespace MultiThreading
{
    internal class StopCommand : ICommand
    {
        private readonly pgeThreadingProgressCancelV2 _parent;
        public event EventHandler CanExecuteChanged;

        public StopCommand(pgeThreadingProgressCancelV2 Parent) => _parent = Parent;

        public bool CanExecute(object parameter) => _parent.IsRunning;

        public void Execute(object parameter) => _parent.StopThreads();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}