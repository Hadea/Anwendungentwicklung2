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
            Console.WriteLine("Startklar?");
            Console.ReadLine();

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

            var dataStream = connection.GetStream();
            while (true)
            {
                Console.Write("Bitte nachricht eingeben: ");
                string message = Console.ReadLine();

                if (message == "stop")
                {
                    break;
                }
                byte[] data = Encoding.ASCII.GetBytes(message);
                dataStream.Write(data, 0, data.Length);
            }
            Console.WriteLine("Client ist dann jetzt wech");
            dataStream.Flush();
            dataStream.Close();
            connection.Close();
            Console.ReadLine();
        }
    }
}
