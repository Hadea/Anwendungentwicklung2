﻿using ChatMessages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServerLogic
{
    public sealed class ChatServer
    {
        private readonly TcpListener listener;
        private readonly LinkedList<ClientConnectionData> connections;
        private readonly LinkedList<ChatRoom> rooms;
        private readonly Action<string> LogOutput;
        private CancellationTokenSource ctsReader;
        private CancellationTokenSource ctsListener;
        private readonly ChatRoom lobby = new();

        public bool IsListenerRunning
        {
            get;
            private set;
        }

        public ChatServer(Action<string> LogMethod)
        {
            LogOutput = LogMethod;
            listener = new(System.Net.IPAddress.Any, 1337);
            ctsReader = new();
            connections = new();
            rooms = new();

            lobby.RoomID = "Eingang";
            rooms.AddFirst(lobby);
        }

        public async void StartListenerAsync()
        {
            if (IsListenerRunning) return;
            ctsListener = new();
            if (ctsReader == null) ctsReader = new();
            IsListenerRunning = true;
            LogOutput("Starte Listener, neue Verbindungen erlaubt");
            listener.Start();
            while (!ctsListener.IsCancellationRequested)
            {
                TcpClient newClient = await Task.Run(() => listener.AcceptTcpClient(), ctsListener.Token).ConfigureAwait(true);//TODO: false müsste genügen
                ClientConnectionData ccd = new();
                ccd.UserName = null;
                ccd.Reader = Task.Run(() => receive(ccd), ctsReader.Token);
                ccd.Connection = newClient;
                connections.AddLast(ccd);
                LogOutput($"neuer Client von {newClient.Client.RemoteEndPoint}");
            }
            LogOutput("Stoppe Listener, keine neuen Verbindungen erlaubt");
            listener.Stop();
            IsListenerRunning = false;
        }

        public void StopListener()
        {
            LogOutput("Listener wird beendet");
            ctsListener.Cancel();
        }

        public void StopReader()
        {
            ctsReader.Cancel();
            foreach (var client in connections)
            {
                LogOutput($"Trenne {(client.UserName ?? "Unangemeldet")} von {client.Connection.Client.RemoteEndPoint}");
                client.Connection.Close();
                client.Reader.Wait();
            }
            connections.Clear();
            LogOutput("Alle Reader und Clients getrennt");
            ctsReader = null;
        }

        public void BroadcastMessage(string Message)
        {
            MessageBroadcast mb = new();
            mb.ContentType = DataType.Text;
            mb.Content = Message.ConvertToArray();

            foreach (var client in connections)
                if (client.Connection.Connected)
                    client.Connection.GetStream().Write(mb.ToArray());
        }

        public IReadOnlyCollection<string> GetConnectionStatus()
        {
            List<string> resultList = new();
            resultList.Add("Status des Servers");
            resultList.Add($"Listener : {IsListenerRunning}");

            int counter = 0;
            foreach (var client in connections)
                resultList.Add($"No: {counter++,2} Name {client.UserName ?? "Unangemeldet"} Connected: {client.Connection.Connected} IP: {client.Connection.Client.RemoteEndPoint} ");

            return resultList;
        }

        private async void receive(ClientConnectionData Client)
        {
            byte[] data = new byte[1024];
            int recievedBytes = 0;

            while (Client.Connection.Connected)
            {
                try
                {
                    recievedBytes = await Client.Connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), ctsReader.Token).ConfigureAwait(true); //TODO false müsste genügen
                    switch ((MessageTypes)data[0])
                    {
                        case MessageTypes.Login:
                            //TODO: check credentials
                            var message = new MessageLogin(data[0..recievedBytes]);
                            bool usernameValid = true;

                            foreach (var client in connections)
                            {
                                if (client.UserName == message.UserName)
                                {
                                    usernameValid = false;
                                    break;
                                }
                            }

                            if (usernameValid)
                            {
                                Client.UserName = message.UserName;
                                MessageLoginSuccessful m = new();
                                Client.Connection.GetStream().Write(m.ToArray());

                                Client.ConnectedRoom = lobby;
                                //TODO: auslagern der MessageUserList erstellung (login und explizite anforderung brauchen so ein paket)
                                MessageUserList mulLogin = new();
                                foreach (var client in connections)
                                    mulLogin.UserList.Add(client.UserName);

                                var arr = mulLogin.ToArray();

                                foreach (var client in connections)
                                {
                                    client.Connection.GetStream().Write(arr);
                                }
                            }
                            else
                            {
                                MessageLoginFail m = new();
                                Client.Connection.GetStream().Write(m.ToArray());
                            }
                            break;
                        case MessageTypes.Logout:
                            Client.Connection.Close();
                            LogOutput($"Client {Client.UserName ?? "Unangemeldet"} hat sich abgemeldet");
                            break;
                        case MessageTypes.DirectMessage:
                            MessageDirect dm = new(data[0..recievedBytes]);
                            foreach (var client in connections)
                                if (client.UserName == dm.DestinationName)
                                {
                                    client.Connection.GetStream().Write(data.AsSpan()[0..recievedBytes]);
                                    break;
                                }
                            // todo: fehlermeldung wenn nachricht unzustellbar
                            break;

                        case MessageTypes.RoomUserList: // client möchte seine nutzerliste aktualisieren
                            // 133 5 RaumA 5RaumB 4Hans0 6Mül1er1 5Meier0 7Schulze1
                            // 133 5 RaumA 2 4Hans 5Meier 5RaumB 2 6Mül1er 7Schulze

                            MessageUserList mul = new();
                            foreach (var client in connections)
                                mul.UserList.Add(client.UserName);
                            Client.Connection.GetStream().Write(mul.ToArray());
                            LogOutput($"Aktualisiere Nutzeransicht für Client {Client.UserName} an IP {Client.Connection.Client.RemoteEndPoint}");
                            break;
                        //todo Nachrichtenarten implementieren
                        case MessageTypes.RoomChange:
                            break;
                        case MessageTypes.RoomMessage:
                            break;
                        case MessageTypes.UserEntersRoom:
                            break;
                        case MessageTypes.UserLeavesRoom:
                            break;
                        case MessageTypes.RoomCreated:
                            break;
                        case MessageTypes.RoomDeleted:
                            break;
                        case MessageTypes.RoomChangeDenied:
                            break;
                        default:
                            LogOutput($"Client {Client.UserName ?? "Unangemeldet"} an IP {Client.Connection.Client.RemoteEndPoint} hat unbekanntes Paket gesendet: {data[0]}  trenne Client");
                            MessageKick mk = new();
                            Client.Connection.GetStream().Write(mk.ToArray());
                            Client.Connection.Close();
                            break;

                    }
                }
                catch (InvalidOperationException)
                {
                    LogOutput("Exeption ausgelöst, beende reader");
                    break;
                }
                if (recievedBytes < 1)
                {
                    LogOutput("Weniger als 1 byte empfangen, beende verbindung");
                    break;
                }
            }
            connections.Remove(Client);
        }

    }


}
