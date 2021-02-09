using System;
using System.Net; // IPAddress
using System.Text; // Encoding
using System.Net.Sockets; // TcpListener und TCPClient

namespace NetServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin der Server");

            // startet den TcpListener welcher auf allen Netzwerkadressen
            // auf diesem Computer auf einen Verbindungsversuch wartet
            // dieser Verbindungsversuch muss über Port 1337 geschehen

            // Der Port darf dabei nicht von einer anderen Anwendung belegt
            // sein und sollte idealerweise nicht unter 1024 sein (reserviert)
            TcpListener listener = new(IPAddress.Any, 1337);
            listener.Start();

            Console.WriteLine("port reserviert, warte auf verbindung");

            // AcceptTcpClient wartet bis ein TCPClient eine Verbindung
            // mit diesem Computer aufbaut und erstellt einen TCPClient
            // auf diesem Server
            // Das TCPClient pärchen repräsentiert die Verbindung durch
            // die nun daten ausgetauscht werden können
            TcpClient connection = listener.AcceptTcpClient();
            Console.WriteLine("Client verbunden");

            // da wir keine weiteren Verbindungsverusche akzeptieren wollen
            // fahren wir den Listener wieder herunter. Weitere 
            // Verbindungsversuche werden jetzt aktiv abgelehnt
            listener.Stop();
            // Der Datenaustausch findet über einen Stream statt welcher
            // mit read und write die daten über das netzwerk schickt und
            // empfängt
            // Da wir als empfänger nicht wissen wie gross die Nachricht
            // des Senders ist erstellen wir ein Array das gross genug
            // für jede nachricht ist und merken uns wieviel von dem Array
            // benutzt wurde
            var stream = connection.GetStream();
            int recievedBytes;
            byte[] data = new byte[1024];

            /* Read wartet auf eine Nachricht und lagert sie in einem Byte
             * Array. Damit Read nicht zu viel auf einmal speichert geben
             * wir die maximale byteanzahl mit an. In unserem fall soll das
             * Array ab stelle 0 für maximal 1024 byte (length) beschrieben
             * werden.
             * Read gibt einen Integer zurück welcher mehrere bedeutungen hat.
             *    grösser 0 : nachricht empfangen und die anzahl der empfangenen bytes
             *    exakt 0: der TCPClient auf der anderen seite hat die verbindung beendet
             *    kleiner 0: es gab einen Netzwerkfehler und die Verbindung ist weg
             * Die while Schleife läuft demnach bis der andere die Verbindung trennt
             */
            while ((recievedBytes = stream.Read(data, 0 , data.Length)) != 0)
            {
                // die empfangene nachricht ist ein Bytearray und muss wieder in
                // das original übersetzt werden (deserialisierung)
                string message = Encoding.ASCII.GetString(data,0,recievedBytes);
                Console.WriteLine("Nachricht vom Client : {0}",message);
            }

            /* Der Client hat die Verbindung getrennt sodass wir aus der while schleife
             * raus sind und können nun selbst unser Netzwerk herunterfahren
             * Wichtig ist dabei die Reihenfolge
             * Wie bei Datenbanken könnten wir das auch über ein using lösen,
             * erfahrungsgemäss sind die methoden aber an so unterschiedlichen
             * stellen im code das es nicht mehr mit einem using geht
             */
            Console.WriteLine("Server ist wech");
            stream.Close();
            connection.Close();
            Console.ReadLine();
        }
    }
}
