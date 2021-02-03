using System;
using System.Windows.Input;

namespace MultiThreading
{
    internal class StopCommand : ICommand
    {
        private pgeThreadingProgressCancelV2 _parent;

        public StopCommand(pgeThreadingProgressCancelV2 Parent)
        {
            _parent = Parent;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _parent.IsRunning;
        }

        public void Execute(object parameter)
        {
            _parent.StopThreads();
        }
    }
}