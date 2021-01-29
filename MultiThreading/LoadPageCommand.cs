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
        public event EventHandler CanExecuteChanged;
        public LoadPageCommand(MainWindow window)
        {
            _parent = window;
        }
        readonly MainWindow _parent;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _parent.frmContent.Navigate(Activator.CreateInstance(Type.GetType(parameter as string)));
        }
    }
}
