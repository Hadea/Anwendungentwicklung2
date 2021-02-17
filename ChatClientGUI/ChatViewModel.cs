using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChatClientGUI
{
    class ChatViewModel : INotifyPropertyChanged
    {
        public ICommand Command_Connect { get; init; }
        public ICommand Command_Send { get; init; }
        public ICommand Command_Refresh { get; init; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RoomViewModel> RoomList { get; } = new();
        public string SelectedTarget { get; set; }
        public Brush ConnectionColor
        {
            get => IsConnected ? Brushes.Green : Brushes.Red;
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    (Command_Connect as GenericParameterCommand).RaiseCanExecuteChanged();
                }
            }
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
                if (_newMessage != value)
                {
                    _newMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewMessage)));
                }
            }
        }
        private string _newMessage;
        public Action ScrollDownMethod;
        private readonly ChatClientLogic.ClientLogic logic;

        public bool IsConnected { get => logic.IsConnected; }
        public bool IsNotConnected { get => !logic.IsConnected; }
        public Dispatcher UIDispatcher { get; internal set; }

        public ChatViewModel()
        {
            Command_Connect = new GenericParameterCommand(connect,canConnect);
            Command_Send = new GenericCommand(sendNewMessage, () => IsConnected);
            Command_Refresh = new GenericCommand(logic.RequestRoomRefresh, () => true);
            logic = new(displayReceivedMessage, displayUserList, connectionStatusChange);
            Messages = string.Empty;
            NewMessage = string.Empty;
            UserName = string.Empty;
        }

        private void connectionStatusChange()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            UIDispatcher.Invoke((Command_Send as GenericCommand).RaiseCanExecuteChanged);
            UIDispatcher.Invoke((Command_Refresh as GenericCommand).RaiseCanExecuteChanged);
        }

        private void sendNewMessage()
        {
            logic.SendMessage(_newMessage);
            NewMessage = string.Empty;
        }

        private void displayReceivedMessage(string ReceivedMessage)
        {
            Messages += Environment.NewLine + ReceivedMessage;
            UIDispatcher.Invoke(ScrollDownMethod);
        }

        private void displayUserList(List<string> RecievedUserList)
        {
            UIDispatcher.Invoke(() =>
            {
                RoomList.Clear();
                foreach (var user in RecievedUserList)
                    RoomList.Add(user);
            });

        }
        private void connect(object param)
        {
            PasswordBox passwordBox = (PasswordBox)param;
            if (IsConnected)
                logic.Stop();
            else
                using (SHA256 hash = SHA256.Create())
                    logic.Start(UserName, hash.ComputeHash(Encoding.ASCII.GetBytes(passwordBox.Password)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            (Command_Send as GenericCommand).RaiseCanExecuteChanged();
        }

        private bool canConnect(object param)
        {
            PasswordBox passwordBox = (PasswordBox)param;
            if (IsConnected)
                return true;

            return UserName.Length > 0 && passwordBox.Password.Length > 0; //TODO: bessere tests
        }
    }
}
