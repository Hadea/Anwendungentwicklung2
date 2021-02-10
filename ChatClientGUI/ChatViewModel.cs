using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatClientGUI
{
    class ChatViewModel : INotifyPropertyChanged
    {
        public ICommand Command_Connect { get; init; }
        public ICommand Command_Send { get; init; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Brush ConnectionColor
        {
            get => IsConnected ? Brushes.Green : Brushes.Red;
        }


        private readonly ChatClientLogic.ClientLogic logic;

        public bool IsConnected { get => logic.IsConnected; }

        public ChatViewModel()
        {
            Command_Connect = new GenericCommand(connect);
            Command_Send = new GenericParameterCommand((x) => logic.SendMessage((string)x), () => IsConnected);
            logic = new();
        }

        private void connect()
        {
            if (IsConnected)
                logic.Stop();
            else
                logic.Start();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            (Command_Send as GenericParameterCommand).RaiseCanExecuteChanged();
        }
    }
}
