using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class AddUserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainViewModel Parent;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Parent.EntryList.Add(new UserViewModel { Name = "neu", Salary = 35000.0 });
        }
    }
}
