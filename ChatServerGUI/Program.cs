using System;

namespace ChatServerGUI
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin ein Chat server Verwaltungsprogramm");
            Console.WriteLine("Sie können jetzt kommandos eingeben");
            Console.WriteLine("   /start    startet den Server und wartet auf Verbindungen");
            Console.WriteLine("   /stop     trennt alle verbindungen und fährt den server herunter");
            Console.WriteLine("   /status   gibt den Verbindungsstatus zurück");

            ChatServerLogic.ChatServer chatServer = new((x) => Console.WriteLine("Empfangen: {0}", x));

            string command;
            while (true)
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "/start":
                        if (chatServer.IsConnected)
                            Console.WriteLine("Client ist bereits verbunden");
                        else
                            chatServer.Start();
                        break;
                    case "/stop":
                        Console.WriteLine("Beende Server");
                        chatServer.Stop();
                        Console.ReadLine();
                        return;
                    case "/status":
                        if (chatServer.IsConnected)
                            Console.WriteLine("Client ist Verbunden");
                        else
                            Console.WriteLine("keine Verbindung");
                        break;
                    default:
                        Console.WriteLine("unbekanntes Kommando : {0}", command);
                        break;
                }
            }
        }
    }
}
