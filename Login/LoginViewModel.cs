using System.ComponentModel;
using System.Windows.Input;

namespace Login
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand UserLogin { get; init; }

        public LoginViewModel()
        {
            UserLogin = new UserLoginCommand(this);
        }

        public string UserLoginResult
        {
            get { return result; }
            set
            {
                if (result != value)
                {
                    result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserLoginResult)));
                }
            }
        }
        private string result;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
