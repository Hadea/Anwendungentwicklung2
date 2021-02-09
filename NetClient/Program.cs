using System;
using System.Net.Sockets; // TCPClient
using System.Text; // Encoding

namespace NetClient
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin der Client");
            Console.WriteLine("Startklar?");
            Console.ReadLine();

            /* Der TCPClient verwaltet eine Verbindung zu einem Computer
             * über TCP/IP.
             * Mit dem Connect befehl stellen wir die Verbindung her
             * dabei geben wir an wo der Computer zu finden ist (IP)
             * und mit welchem Dienst wir verbunden werden wollen (Port)
             * 
             * Sollte es dabei zu Problemen kommen (Firewall, Falsche IP,...)
             * wird eine Exeption geworfen welche details enthält.
             * 
             * In Connected steht ob die Verbindung noch immer besteht
             */
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

            /* Die Kommunikation wird über einen Stream gehandhabt welcher
             * Read und Write befehle über den TCPClient mit dem anderen
             * Computer abwickelt.
             */
            var dataStream = connection.GetStream();
            while (true)
            {
                Console.Write("Bitte nachricht eingeben: ");
                string message = Console.ReadLine();

                // solange der nutzer nicht stop tippt wird die eingabe gesendet
                if (message == "stop")
                {
                    break;
                }

                /* Die Übertragung zum anderen Computer wird mit byte-Arrays
                 * bewerkstelligt. Dazu müssen wir den String erst Konvertieren
                 * (serialisieren).
                 * C# liefert bereits einige tools dafür mit. In unserem Fall
                 * genügt GetBytes.
                 * 
                 * Der Convertierte String wird dann mit Write übertragen wobei
                 * immer auch der Ausschnitt des Arrays angegeben wird welcher
                 * die Nachricht enthält. Dies ist besonders nützlich wenn die
                 * zu übertragende Nachricht sehr gross ist und in einzelnen
                 * schritten übertragen werden muss.
                 */
                byte[] data = Encoding.ASCII.GetBytes(message);
                dataStream.Write(data, 0, data.Length);
            }

            /* Wenn der Nutzer stop getippt hat verlassen wir die Schleife
             * und fahren unsere Seite der Verbindung herunter.
             * Die Reihenfolge ist dabei wichtig.
             * Durch den TCPClient.Close() befehl wird dem Server eine
             * Nachricht übermittelt das die Verbindung regulär beendet
             * wird.
             */
            Console.WriteLine("Client ist dann jetzt wech");
            dataStream.Flush();
            dataStream.Close();
            connection.Close();
            Console.ReadLine();
        }
    }
}
