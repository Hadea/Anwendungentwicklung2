using System;
using System.Net.Sockets;
using System.Text;

namespace NetClient
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin der Client");

            TcpClient connection = new();
            connection.Connect("127.0.0.1", 1337);
            if (connection.Connected)
            {
                Console.WriteLine("Jup, sind verbunden"); 
            }
            else
            {
                Console.WriteLine("war nix");
                Console.ReadLine();
                return;
            }

            Console.Write("Bitte nachricht eingeben: ");
            string message = Console.ReadLine();
            byte[] data = Encoding.ASCII.GetBytes(message);
            var dataStream = connection.GetStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Flush();
            dataStream.Close();
            connection.Close();
            Console.ReadLine();
        }
    }
}
