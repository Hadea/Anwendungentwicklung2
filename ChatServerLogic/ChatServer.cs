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
        /*
         Action          <= ohne parameter und ohne rückgabe
         Action<bool>    <= mit bool parameter und ohne rückgabe
         Func<bool>      <= ohne parameter mit bool rückgabe
         
         */

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

        public async Task StartAsync()
        {
            listener.Start();
            connection = await Task<TcpClient>.Run(() => listener.AcceptTcpClient());
            listener.Stop();
            cts = new();
            _ = Task.Run(receive, cts.Token);
        }
        public void Start()
        {
            listener.Start();
            connection = listener.AcceptTcpClient();
            onMessageReceived.Invoke("Client verbindet sich");
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

        private async void receive()
        {
            string message;
            byte[] data = new byte[1024];
            int recievedBytes =0;
            var dataStream = connection.GetStream();

            while (true)
            {

                try
                {
                    recievedBytes = await dataStream.ReadAsync(data.AsMemory(0, data.Length), cts.Token);
                }
                catch (Exception)
                {
                    onMessageReceived.Invoke("Cancel Token wurde ausgelöst");
                    break;
                }
                if (recievedBytes < 1)
                {
                    onMessageReceived.Invoke("Weniger als 1 byte empfangen, beende verbindung");
                    break;
                }
                message = Encoding.ASCII.GetString(data, 0, recievedBytes);
                onMessageReceived.Invoke(message);
                Thread.Sleep(100);
            }
            onMessageReceived.Invoke("Beende reciever");
            dataStream.Close();
        }
    }
}
