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
            Console.WriteLine("   /send <Message>  sendet eine Nachricht an den Client");

            ChatServerLogic.ChatServer chatServer = new((x) => Console.WriteLine("Empfangen: {0}", x));

            string input;
            string command;
            while (true)
            {
                input = Console.ReadLine();
                int spaceID = input.IndexOf(" ");
                command = input.Substring(0, (spaceID > 0 ? spaceID : input.Length));

                switch (command)
                {
                    case "/start":
                        if (chatServer.IsConnected)
                            Console.WriteLine("Client ist bereits verbunden");
                        else
                            chatServer.StartAsync();
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
                    case "/send":
                        chatServer.SendMessage(input[6..]);
                        break;
                    default:
                        Console.WriteLine("unbekanntes Kommando : {0}", command);
                        break;
                }
            }
        }
    }
}
