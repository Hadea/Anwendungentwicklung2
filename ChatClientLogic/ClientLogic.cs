using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClientLogic
{
    public class ClientLogic
    {
        TcpClient connection;
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

        public bool Start()
        {
            connection = new TcpClient();

            try
            {
                connection.Connect("127.0.0.1", 1337);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void SendMessage(string Message)
        {
            if (connection == null || !connection.Connected) return;
            byte[] data;
            data = Encoding.ASCII.GetBytes(Message);
            connection.GetStream().Write(data, 0, data.Length);
        }

        public void Stop()
        {
            connection?.Close();
        }
    }
}
