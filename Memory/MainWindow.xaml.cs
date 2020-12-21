using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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

        List<BitmapImage> bitmaps;

        public MainWindow()
        {
            InitializeComponent();


            bitmaps = new List<BitmapImage>();

            // alle bilder laden
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit();// füllen des Bildes starten
                tempBitmap.UriSource = new Uri( Directory.GetParent(Environment.CommandLine).FullName + @"\" + fileName);// bildinhalt aus datei laden
                tempBitmap.EndInit();// füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }

        }

        void createGame(int Columns, int Rows)
        {
            // clear

            Spielfeld.Children.Clear();
            Spielfeld.ColumnDefinitions.Clear();
            Spielfeld.RowDefinitions.Clear();

            // Linkedliste mit erlaubten image ziffern erstellen
            List<int> availableBitmaps = new List<int>();
            for (int counter = 0; counter < bitmaps.Count; counter++)
                availableBitmaps.Add(counter);

            List<Image> images = new List<Image>();
            // recreate
            var gridElementSize = new GridLength(100);

            for (int counter = 0; counter < Columns; counter++)
            {
                ColumnDefinition colDef = new();
                colDef.Width = gridElementSize;
                Spielfeld.ColumnDefinitions.Add(colDef);
            }


            for (int counter = 0; counter < Rows; counter++)
            {
                RowDefinition rowDef = new();
                rowDef.Height = gridElementSize;
                Spielfeld.RowDefinitions.Add(rowDef);
            }

            Random rndGen = new();
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    // Image tag erstellen
                    Image tempImage = new Image
                    {
                        Stretch = Stretch.Uniform,
                    };
                    images.Add(tempImage);
                    // Button erstellen und füllen
                    Button temp = new();
                    temp.Content = tempImage; // button mit Image füllen


                    // Im Grid eintragen
                    Grid.SetColumn(temp, col); // button in spalte positionieren
                    Grid.SetRow(temp, row); // button in zeile positionieren
                    Spielfeld.Children.Add(temp);
                }
            }


            for (int counter = 0; counter < Columns * Rows / 2; counter++)
            {
                int choosenBitmap = rndGen.Next(availableBitmaps.Count);
                int chosenImage;
                chosenImage = rndGen.Next(images.Count);
                images[chosenImage].Source = bitmaps[availableBitmaps[choosenBitmap]];
                images.RemoveAt(chosenImage);

                chosenImage = rndGen.Next(images.Count);
                images[chosenImage].Source = bitmaps[availableBitmaps[choosenBitmap]];
                images.RemoveAt(chosenImage);

                availableBitmaps.RemoveAt(choosenBitmap);
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            createGame(int.Parse(tbWidth.Text), int.Parse(tbHeight.Text));
        }
    }
}
