using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Login
{
    internal class UserLoginCommand : ICommand
    {
        private readonly LoginViewModel parent;

        public UserLoginCommand(LoginViewModel loginViewModel)
        {
            parent = loginViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            object[] paramConverted = parameter as object[];
            if (paramConverted[0] as string == "Admin" && (paramConverted[1] as PasswordBox).Password == "123456")
                parent.UserLoginResult = "Willkommen " + paramConverted[0];
            else
                parent.UserLoginResult = "Nope!";
        }
    }
}