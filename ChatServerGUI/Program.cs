using System;
using System.Collections.Generic;

namespace ChatServerGUI
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin ein Chat server Verwaltungsprogramm");
            Console.WriteLine("Sie können jetzt kommandos eingeben");
            Console.WriteLine("   /start            startet den Server und wartet auf Verbindungen und startet reader");
            Console.WriteLine("   /stop             stopt den listener");
            Console.WriteLine("   /status           gibt den Serverstatus zurück");
            Console.WriteLine("   /send <Message>   sendet eine Nachricht an alle Clients");
            Console.WriteLine("   /send <Message>   sendet eine Nachricht an alle Clients");
            Console.WriteLine("   /shutdown         stoppt den listener, trennt alle verbindungen und beendet das Programm");

            ChatServerLogic.ChatServer chatServer = new((x) => Console.WriteLine("Empfangen: {0}", x));

            string input;
            string command;
            while (true)
            {
                input = Console.ReadLine();
                int spaceID = input.IndexOf(" ");
                command = input.Substring(0, (spaceID > 0 ? spaceID : input.Length));
                //todo: add parameter

                switch (command)
                {
                    case "/start":
                        chatServer.StartListenerAsync();
                        break;
                    case "/stop":
                        chatServer.StopListener();
                        break;
                    case "/status":
                        List<string> status = chatServer.GetConnectionStatus();
                        foreach (var item in status) Console.WriteLine(item);
                        break;
                    case "/send":
                        chatServer.SendMessage(input[6..]);
                        break;
                    case "/shutdown":
                        Console.WriteLine("Beende anwendung");
                        chatServer.StopListener();
                        chatServer.Kick();
                        return;
                    default:
                        Console.WriteLine("unbekanntes Kommando : {0}", command);
                        break;
                }
            }
        }
    }
}
