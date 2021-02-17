using ChatMessages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClientLogic
{
    public sealed class ClientLogic : IDisposable
    {
        private TcpClient connection;
        private readonly Action<string> onNewMessage;
        private readonly Action<List<string>> onNewUserList;
        private readonly Action onConnectionStatusChanged;
        private CancellationTokenSource cts;
        public bool IsConnected
        {
            get
            {
                if (connection != null && connection.Connected)
                    return true;
                else
                    return false;
            }
        }

        public bool IsLoggedIn
        {
            get;
            private set;
        }

        public ClientLogic(Action<string> OnNewMessage, Action<List<string>> OnNewUserList, Action OnConnectionStatusChanged)
        {
            onNewMessage = OnNewMessage;
            onNewUserList = OnNewUserList;
            onConnectionStatusChanged = OnConnectionStatusChanged;
        }

        public bool Start(string UserName, byte[] Password)
        {
            connection = new TcpClient();

            try
            {
                connection.Connect("127.0.0.1", 1337);
            }
            catch (SocketException)
            {
                return false;
            }
            cts = new();
            _ = Task.Run(recieve, cts.Token);
            MessageLogin ml = new();

            ml.Password = Password;
            ml.UserName = UserName;
            connection.GetStream().Write(ml.ToArray());
            return true;
        }

        public void SendMessage(string Message)
        {
            if (connection == null || !connection.Connected) return;
            MessageRoom message = new();
            message.ContentType = DataType.Text;
            message.Content = Message.ConvertToArray();
            connection.GetStream().Write(message.ToArray());
        }

        public void Stop()
        {
            cts.Cancel();
            connection?.Close();
        }

        private async void recieve()
        {
            byte[] data = new byte[1024];
            int receivedBytes;

            while (connection.Connected)
            {
                try
                {
                    receivedBytes = await connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), cts.Token).ConfigureAwait(true); //todo false könnte genügen
                }
                catch (InvalidOperationException)
                {
                    // wenn fehler bei der übertragung stattfinden (server down, netzwerk down)
                    connection.Close();
                    break;
                }
                if (receivedBytes < 1)
                {
                    // server hat verbindung regulär getrennt
                    connection.Close();
                    break;
                }
                else
                {
                    // nachricht empfangen
                    switch ((MessageTypes)data[0])
                    {
                        case MessageTypes.LoginSuccessful:
                            IsLoggedIn = true;
                            onNewMessage.Invoke("Server: Login erfolgreich");
                            break;
                        case MessageTypes.LoginFail:
                            IsLoggedIn = false;
                            onNewMessage.Invoke("Server: Login abgelehnt"); //todo grund angeben
                            break;
                        case MessageTypes.KickFromServer:
                            IsLoggedIn = false;
                            connection.Close();
                            onNewMessage.Invoke("Server: Sie wurden vom Server getrennt");
                            break;
                        case MessageTypes.ServerShutdown:
                            IsLoggedIn = false;
                            connection.Close();
                            onNewMessage("Server: Heruntergefahren");
                            break;
                        case MessageTypes.DirectMessage:
                            MessageDirect md = new(data[0..receivedBytes]);
                            if (md.ContentType == DataType.Text)
                                onNewMessage("Direktnachricht " + md.SourceName + "> " + md.Content.ConvertToString());
                            else
                                // todo: bild und datei-daten
                                throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                            break;
                        case MessageTypes.RoomMessage:
                            MessageRoom mr = new(data[0..receivedBytes]);
                            if (mr.ContentType == DataType.Text)
                                onNewMessage(mr.SourceName + "> " + mr.Content.ConvertToString());
                            else
                                // todo: bild und datei-daten
                                throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                            break;
                        case MessageTypes.Broadcast:
                            MessageRoom mb = new(data[0..receivedBytes]);
                            if (mb.ContentType == DataType.Text)
                                onNewMessage("Server-Broadcast> " + mb.Content.ConvertToString());
                            else
                                // todo: bild und datei-daten
                                throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                            break;
                        case MessageTypes.BanFromServer:
                            IsLoggedIn = false;
                            connection.Close();
                            onNewMessage.Invoke("Server: Sie wurden vom Server verbannt");
                            break;
                        case MessageTypes.RoomUserList:
                            MessageUserList mul = new(data[..receivedBytes]);
                            onNewUserList(mul.UserList);
                            break;
                        default:
                            //todo: mehr nachrichten implementieren
                            throw new NotImplementedException($"Nachrichtenart {(MessageTypes)data[0]} nicht unterstützt");
                    }
                }
            }
            onConnectionStatusChanged();
        }

        public void RequestUserRefresh()
        {
            MessageRequestUserList m = new();
            connection.GetStream().Write(m.ToArray());
        }

        public void Dispose()
        {
            connection.Dispose();
            cts.Dispose();

        }
    }
}
