using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace ChatServerLogic
{
    public class ChatServer
    {
        private readonly TcpListener listener;
        private LinkedList<TcpClient> connections;


        private readonly Action<string> onMessageReceived;
        CancellationTokenSource ctsReader;
        CancellationTokenSource ctsListener;

        public bool IsListenerRunning
        {
            get => _listenerRunning;
        }
        bool _listenerRunning;
        public bool IsReaderRunning
        {
            get => _readerRunning;
        }
        bool _readerRunning;

        public ChatServer(Action<string> RecieveMethod)
        {
            onMessageReceived = RecieveMethod;
            listener = new(System.Net.IPAddress.Any, 1337);
            ctsReader = new();
            ctsListener = new();
            connections = new();
        }

        public async void StartListenerAsync()
        {
            if (_listenerRunning) return;
            _listenerRunning = true;
            onMessageReceived.Invoke("Starte Listener");
            listener.Start();
            while (!ctsListener.IsCancellationRequested)
            {
                TcpClient newClient = await Task.Run(() => listener.AcceptTcpClient(), ctsListener.Token);
                _ = Task.Run(() => receive(newClient), ctsReader.Token);
                connections.AddLast(newClient);
                onMessageReceived.Invoke("Client verbindet sich");
            }
            onMessageReceived.Invoke("Stoppe Listener");
            listener.Stop();
            _listenerRunning = false;
        }

        public void StopListener()
        {
            onMessageReceived.Invoke("Listener wird beendet");
            ctsListener.Cancel();
        }

        public void StopReader()
        {
            onMessageReceived.Invoke("Reader wird beendet");
            ctsReader.Cancel();
        }

        public void Kick()
        {
            ctsReader.Cancel();
            foreach (var client in connections)
                client.Close();
            connections.Clear();
        }

        public void SendMessage(string Message)
        {
            byte[] data;
            data = Encoding.ASCII.GetBytes(Message);
            foreach (var client in connections)
                if (client.Connected)
                    client.GetStream().Write(data, 0, data.Length);
        }

        public List<string> GetConnectionStatus()
        {
            List<string> resultList = new();
            resultList.Add("Status des Servers");
            resultList.Add($"Reader : {IsReaderRunning}");
            resultList.Add($"Writer : {IsListenerRunning}");

            int counter = 0;
            foreach (var client in connections)
                resultList.Add($"ClientNummer: {counter++} Connected: {client.Connected} IP: {client.Client.RemoteEndPoint} ");

            return resultList;
        }

        private async void receive(TcpClient Client)
        {
            string message;
            byte[] data = new byte[1024];
            int recievedBytes = 0;

            while (true)
            {
                try
                {
                    recievedBytes = await Client.GetStream().ReadAsync(data.AsMemory(0, data.Length), ctsReader.Token);
                    message = Encoding.ASCII.GetString(data, 0, recievedBytes);
                    onMessageReceived.Invoke(message);
                    SendMessage(message);
                }
                catch (Exception)
                {
                    onMessageReceived.Invoke("Exeption ausgelöst, beende reader");
                    break;
                }
                if (recievedBytes < 1)
                {
                    onMessageReceived.Invoke("Weniger als 1 byte empfangen, beende verbindung");
                    break;
                }
            }
            Client.Close();
            connections.Remove(Client);
        }
    }
}
