using System;
using System.Windows.Input;

namespace MultiThreading
{
    class StartCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly pgeThreadingProgressCancelV2 _parent;
        public StartCommand(pgeThreadingProgressCancelV2 Parent)
        {
            _parent = Parent;
        }
        public bool CanExecute(object parameter)
        {
            return _parent.IsNotRunning;
        }

        public void Execute(object parameter)
        {
            switch (parameter as string)
            {
                case "SplitByThread":
                    _parent.StartThreadSplit();
                    break;
                case "SplitByUser":
                    _parent.StartUserSplit();
                    break;
                default:
                    throw new ArgumentException("Unknown version of Start");
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
