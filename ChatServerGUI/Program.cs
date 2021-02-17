using System;

string[] strings = {"Hallo, ich bin ein Chat server Verwaltungsprogramm",
                "Sie können jetzt kommandos eingeben",
                "   /start            startet den Server und wartet auf Verbindungen und startet reader",
                "   /stop             stopt den listener",
                "   /status           gibt den Serverstatus zurück",
                "   /send <Message>   sendet eine Nachricht an alle Clients",
                "   /shutdown         stoppt den listener, trennt alle verbindungen und beendet das Programm",
                "Beende anwendung"};

for (int counter = 0; counter < 6; counter++)
    Console.WriteLine(strings[counter]);

ChatServerLogic.ChatServer chatServer = new((x) => Console.WriteLine("Empfangen: {0}", x));

string input;
string command;
while (true)
{
    input = Console.ReadLine();
    int spaceID = input.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
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
            var status = chatServer.GetConnectionStatus();
            foreach (var item in status) Console.WriteLine(item);
            break;
        case "/send":
            chatServer.BroadcastMessage(input[6..]);
            break;
        case "/shutdown":
#pragma warning disable IDE0079 //Remove unnecessary suppression
#pragma warning disable CA1303  // Do not pass literals as localized parameters
            Console.WriteLine(strings[6]);
#pragma warning restore CA1303, IDE0079 // Do not pass literals as localized parameters // Remove unnecessary suppression
            chatServer.StopListener();
            chatServer.StopReader();
            return;
        default:
            Console.WriteLine($"unbekanntes Kommando : {command}");
            break;
    }
}
