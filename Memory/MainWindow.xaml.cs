using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<BitmapImage> bitmaps;
        private Button selectedButtonA = null;
        private Button selectedButtonB = null;
        private DateTime gameStart;
        private bool gameRunning = false;
        private readonly DispatcherTimer timer;

        public int Turns { get => turns; set { turns = value; lblTurns.Content = "Turns: " + turns.ToString(); } }
        private int turns = 0;

        public int Points { get => points; set { points = value; lblPoints.Content = "Points: " + points.ToString(); } }
        private int points = 0;


        public MainWindow()
        {
            InitializeComponent();

            bitmaps = new List<BitmapImage>();

            // alle bilder laden
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit();// füllen des Bildes starten
                tempBitmap.UriSource = new Uri(Directory.GetParent(Environment.CommandLine).FullName + @"\" + fileName);// bildinhalt aus datei laden
                tempBitmap.EndInit();// füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }

            timer = new DispatcherTimer(new TimeSpan(0,0,0,0,50), DispatcherPriority.Background, displayTime, Dispatcher.CurrentDispatcher);
        }

        void displayTime(object o, EventArgs e)
        {
            if (gameRunning)
            {
                lblTime.Content = "Zeit: " + (DateTime.Now - gameStart).TotalSeconds.ToString("N3");
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
            GridLength gridElementSize = new(100);

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

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    // Image tag erstellen
                    Image tempImage = new Image
                    {
                        Stretch = Stretch.Uniform,
                        Visibility = Visibility.Hidden
                    };
                    images.Add(tempImage);
                    // Button erstellen und füllen
                    Button temp = new();
                    temp.Style = FindResource("FieldButton") as Style;
                    temp.Content = tempImage; // button mit Image füllen

                    // Im Grid eintragen
                    Grid.SetColumn(temp, col); // button in spalte positionieren
                    Grid.SetRow(temp, row); // button in zeile positionieren
                    Spielfeld.Children.Add(temp);
                }
            }


            Random rndGen = new();
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

        List<Score> readDatabase()
        {

            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "config.db";
            builder.Version = 3;
            builder.FailIfMissing = true;

            List<Score> highscore = new();

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "select Name, SolveTime, row_number() over ( order by SolveTime asc ) rank from Scores where TileNumber = @tiles limit 10;";
                command.Parameters.AddWithValue("tiles", Spielfeld.Children.Count);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Score temp = new();
                        temp.Name = reader.GetString(0);
                        temp.Time = reader.GetDouble(1).ToString("N3");
                        temp.Rank = reader.GetInt32(2);
                        highscore.Add(temp);
                    }
                }
            }
            return highscore;
        }
        void addEntryToDatabase(int Tiles, double totalMilliseconds, string PlayerName)
        {
            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "config.db";
            builder.Version = 3;
            builder.FailIfMissing = true;

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "insert into Scores (Name, SolveTime, TileNumber) values (@name, @time, @number);";
                command.Parameters.AddWithValue("name", PlayerName);
                command.Parameters.AddWithValue("time", totalMilliseconds);
                command.Parameters.AddWithValue("number", Tiles);

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            createGame(int.Parse(tbWidth.Text), int.Parse(tbHeight.Text)); //Hack: check values!
            selectedButtonA = null;
            selectedButtonB = null;
            gameRunning = false;
            Points = 0;
            Turns = 0;
        }

        private void btnField_Click(object sender, RoutedEventArgs e)
        {
            if (!gameRunning)
            {
                gameRunning = true;
                gameStart = DateTime.Now;
            }

            // zwei bereits sichtbar => alle verstecken und austragen
            if (selectedButtonB != null)
            {
                (selectedButtonB.Content as Image).Visibility = Visibility.Hidden;
                (selectedButtonA.Content as Image).Visibility = Visibility.Hidden;
                selectedButtonA = null;
                selectedButtonB = null;
            }

            ((sender as Button).Content as Image).Visibility = Visibility.Visible;

            // prüfen ob es der erste oder der zweite button ist welcher aufgedeckt wird
            if (selectedButtonA == null)
            {
                // erster button
                selectedButtonA = sender as Button;
                (selectedButtonA.Content as Image).Visibility = Visibility.Visible;
            }
            else
            {
                Turns++;
                // zweiter button
                if ((selectedButtonA.Content as Image).Source == ((sender as Button).Content as Image).Source)
                {
                    // wenn inhalt gleich mit dem vom ersten
                    selectedButtonA.IsEnabled = false;
                    (sender as Button).IsEnabled = false;
                    //      punkte geben
                    Points++;
                    //      beide deaktivieren
                    selectedButtonA = null;
                    selectedButtonB = null;
                    if (Points == Spielfeld.Children.Count / 2)
                    {
                        // alle felder gelöst
                        gameRunning = false;
                        // datenbank füllen
                        addEntryToDatabase(Spielfeld.Children.Count, (DateTime.Now - gameStart).TotalSeconds, "Name");
                        // statistik laden
                        List<Score> highscore = readDatabase();
                        // win-screen anzeigen
                        Spielfeld.Children.Clear();
                        Spielfeld.RowDefinitions.Clear();
                        Spielfeld.ColumnDefinitions.Clear();

                        DataGrid dg = new();
                        dg.ItemsSource = highscore;
                        Spielfeld.Children.Add(dg);
                    }
                }
                else
                {
                    // wenn inhalt ungleich zum ersten dann als zweiten offenen eintragen
                    selectedButtonB = sender as Button;
                }
            }
        }
    }
}
