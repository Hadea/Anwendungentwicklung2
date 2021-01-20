using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    class ModifyUserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainViewModel Parent;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.EntryList[(int)parameter].Name = "geändert";
        }
    }
}
