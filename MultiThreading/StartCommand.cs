using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            return ! _parent.IsRunning;
        }

        public void Execute(object parameter)
        {
            _parent.Start();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
