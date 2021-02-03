using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiThreading
{
    class LoadPageCommand : ICommand
    {
        readonly MainWindow _parent;
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public LoadPageCommand(MainWindow window)
        {
            _parent = window;
        }
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _parent.frmContent.Navigate(Activator.CreateInstance(Type.GetType(parameter as string)));
        }

    }
}
