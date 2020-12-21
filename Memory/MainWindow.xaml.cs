using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Spielfeld.Children.Add(new Button());

            var c = (Content as Grid);
            Grid innerGrid;
            foreach (var item in c.Children)
            {
                if ((item is Grid))
                {
                    innerGrid = (item as Grid);
                    break;
                }
            }

        }


        void readDatabase()
        {

            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "./highscore.db";
            builder.Version = 3;
            /*
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(); // Der Builder kann uns den passenden Connectionstring zusammensetzen sodass Syntaxfehler minimiert werden
            connectionStringBuilder.Server = "192.168.2.2"; // IP Adresse des servers, DNS name, Localhost und . funktioniert auch
            connectionStringBuilder.UserID = "MusicDBUser"; // Benutzername innerhalb des DBMS, dieser nutzer sollte so wenig rechte wie möglich bekommen
            connectionStringBuilder.Password = "MusicDBPass"; // Passwort zu dem Benutzernamen
            connectionStringBuilder.Database = "musicdb"; // Datenbankname mit der sich verbunden werden soll, alle SQL statements sind dann relativ zu dieser Datenbank (siehe USE )
            connectionStringBuilder.SslMode = MySqlSslMode.None; // None ist ok für testumgebungen, im Internet immer verschlüsseln. Benötigt extra CPU-Leistung
            */

            List<Pair> highscore = new();

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "select rowid, Name, Points from Scores order by Points desc top 10;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // angenommende Tabelle:
                        // ID, Name, Points
                        // 1 , Hans, 20
                        // 2 , Lisa, 18
                        Pair temp = new();
                        temp.Rank = reader.GetInt32(0);
                        temp.Name = reader.GetString(1);
                        temp.Points = reader.GetInt32(2);
                        highscore.Add(temp);
                    }
                }
            }
            
            void addEntryToDatabase(int Points, string PlayerName)
            {
                // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
                SQLiteConnectionStringBuilder builder = new();
                builder.DataSource = "./highscore.db";
                builder.Version = 3;

                using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "insert into Scores (Name, Points) values (@name, @points);";
                    command.Parameters.AddWithValue("name", PlayerName);
                    command.Parameters.AddWithValue("points", Points);

                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception();
                }
            }
        }
    }
}
