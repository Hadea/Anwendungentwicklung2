using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace NetServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin der Server");
            TcpListener listener = new(IPAddress.Any, 1337);
            Console.WriteLine("port reserviert, warte auf verbindung");
            listener.Start();

            TcpClient connection = listener.AcceptTcpClient();
            Console.WriteLine("Client verbunden");

            var stream = connection.GetStream();
            int recievedBytes;
            byte[] data = new byte[1024];
            while ((recievedBytes = stream.Read(data, 0 , data.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(data);
                Console.WriteLine("Nachricht vom Client : {0}",message);
            }

            stream.Close();
            connection.Close();
            listener.Stop();
            Console.ReadLine();
        }
    }
}
