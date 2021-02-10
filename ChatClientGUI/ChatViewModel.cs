using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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

        public string Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
                }
            }
        }
        private string _messages;

        public string NewMessage
        {
            get => _newMessage;
            set
            {
                if (_newMessage!= value)
                {
                    _newMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewMessage)));
                }
            }
        }
        private string _newMessage;

        private readonly ChatClientLogic.ClientLogic logic;

        public bool IsConnected { get => logic.IsConnected; }

        public ChatViewModel()
        {
            Command_Connect = new GenericCommand(connect);
            Command_Send = new GenericCommand(() => { logic.SendMessage(_newMessage); Messages += Environment.NewLine + "You  > " + _newMessage; NewMessage = String.Empty; }, () => IsConnected);
            logic = new(displayRecievedMessage);
            Messages = string.Empty;
            NewMessage = string.Empty;
        }

        private void displayRecievedMessage(string NewMessage)
        {
            Messages += Environment.NewLine + "Other> " + NewMessage;
        }
        private void connect()
        {
            if (IsConnected)
                logic.Stop();
            else
                logic.Start();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            (Command_Send as GenericCommand).RaiseCanExecuteChanged();
        }
    }
}
