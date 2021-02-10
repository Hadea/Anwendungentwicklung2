using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace ChatServerLogic
{
    public class ChatServer
    {
        private readonly TcpListener listener;
        private TcpClient connection;
        

        private readonly Action<string> onMessageReceived;
        CancellationTokenSource cts;
        public bool IsConnected
        {
            get
            {
                return connection != null && connection.Connected;
            }
        }

        public ChatServer(Action<string> RecieveMethod)
        {
            onMessageReceived = RecieveMethod;
            listener = new(System.Net.IPAddress.Any, 1337);
        }

        public async void StartAsync()
        {
            listener.Start();
            connection = await Task.Run(() => listener.AcceptTcpClient());
            onMessageReceived.Invoke("Client verbindet sich, stoppe Listener");
            listener.Stop();
            cts = new();
            _ = Task.Run(receive, cts.Token);
        }
        public void Start()
        {
            listener.Start();
            connection = listener.AcceptTcpClient();
            onMessageReceived.Invoke("Client verbindet sich, stoppe Listener");
            listener.Stop();
            cts = new();
            _ = Task.Run(receive, cts.Token);
        }

        public void Stop()
        {
            onMessageReceived.Invoke("Server wird beendet");
            cts.Cancel();
            connection.Close();
        }

        public void SendMessage(string Message)
        {
            if (connection == null || !connection.Connected) return;
            byte[] data;
            data = Encoding.ASCII.GetBytes(Message);
            connection.GetStream().Write(data, 0, data.Length);
        }

        private async void receive()
        {
            string message;
            byte[] data = new byte[1024];
            int recievedBytes = 0;

            while (true)
            {

                try
                {
                    recievedBytes = await connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), cts.Token);
                    message = Encoding.ASCII.GetString(data, 0, recievedBytes);
                    onMessageReceived.Invoke(message);
                }
                catch (Exception)
                {
                    onMessageReceived.Invoke("Cancel Token wurde ausgelöst");
                    onMessageReceived.Invoke("Beende receiver");
                    return;
                }
                if (recievedBytes < 1)
                {
                    onMessageReceived.Invoke("Weniger als 1 byte empfangen, beende verbindung");
                    connection.Close();
                    onMessageReceived.Invoke("Warte auf neue Verbindung vom Client");
                    listener.Start();
                    connection = listener.AcceptTcpClient();
                    onMessageReceived.Invoke("Client wieder verbunden");
                }
            }
        }
    }
}
