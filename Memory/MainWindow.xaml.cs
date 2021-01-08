using System;
using System.Collections.Generic;
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
        private List<BitmapImage> bitmaps; // enthält alle Bilder
        private Button selectedButtonA = null;
        private Button selectedButtonB = null;
        private DateTime gameStart;
        private readonly IHighScoreStorage highScore;
        private DispatcherTimer Timer
        {
            get
            {
                if (timer == null) // wenn noch kein timer erstellt ist machen wir das jetzt "lazy initialization"
                    timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50),
                        DispatcherPriority.Background,
                        (_, _) => lblTime.Content = "Zeit: " + (DateTime.Now - gameStart).TotalSeconds.ToString("N1"), // Unterstrich _ für die Parameternamen bedeutet hier das die Parameter verworfen werden können da sie im Lambda nicht verwendet werden
                        Dispatcher.CurrentDispatcher);
                return timer;
            }
        }
        private DispatcherTimer timer;

        public int Turns
        {
            get => turns;
            set
            {
                turns = value;
                lblTurns.Content = "Turns: " + turns.ToString();
            }
        }
        private int turns = 0;

        public int Points
        {
            get => points;
            set
            {
                points = value;
                lblPoints.Content = "Points: " + points.ToString();
            }
        }
        private int points = 0;

        /// <summary>
        /// Creates the Game Window, loads all possible images and prepares timing.
        /// </summary>
        public MainWindow(IHighScoreStorage highScoreStorage)
        {
            InitializeComponent();
            loadAllBitmapimages();
            Timer.Stop();
            // übergebenes Objekt, egal von welchem Typ es ist, wird intern gespeichert und benutzt.
            // Es muss nur kompatibel zum Interface sein. "Dependency Injection"
            highScore = highScoreStorage; 
        }

        /// <summary>
        /// Creates a list of BitmapImage and fills it with all images contained in the ./Images folder
        /// </summary>
        private void loadAllBitmapimages()
        {
            bitmaps = new List<BitmapImage>();
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit();// füllen des Bildes starten
                tempBitmap.UriSource = new Uri(Directory.GetParent(Environment.CommandLine).FullName + @"\" + fileName);// bildinhalt aus datei laden
                tempBitmap.EndInit();// füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }
        }

        /// <summary>
        /// Creates a new game in the specified size. Size should be divisable by 2.
        /// </summary>
        /// <param name="Columns">Number of tile columns</param>
        /// <param name="Rows">Number of tile rows</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if number of field elements not even</exception>
        void createGame(int Columns, int Rows)
        {
            if (Columns * Rows % 2 != 0) throw new ArgumentOutOfRangeException();

            Spielfeld.Clear();

            // spalten und zeilen im Grid erstellen
            GridLength gridElementSize = new(100); // einheitliche längenangabe welche für breite und höhe der spielfelder benutzt wird

            for (int counter = 0; counter < Columns; counter++)
            {
                ColumnDefinition colDef = new();// neue Spalte erzeugen
                colDef.Width = gridElementSize; // Grösse der Spalte eingeben
                Spielfeld.ColumnDefinitions.Add(colDef); // Spalte im Grid eintragen
            }

            for (int counter = 0; counter < Rows; counter++)
            {
                RowDefinition rowDef = new();
                rowDef.Height = gridElementSize;
                Spielfeld.RowDefinitions.Add(rowDef);
            }

            // erstellen der buttons in den Grid-Zellen
            List<Image> images = new List<Image>();
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

            // Liste mit erlaubten image ziffern erstellen
            List<int> availableBitmaps = new List<int>();
            for (int counter = 0; counter < bitmaps.Count; counter++)
                availableBitmaps.Add(counter);

            // zwei zufällige Images mit einem zufälligen BitmapImage füllen
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            int width, height;

            try
            {
                width = int.Parse(tbWidth.Text);
                height = int.Parse(tbHeight.Text);
                createGame(width, height);
                selectedButtonA = null;
                selectedButtonB = null;
                Timer.Stop();
                lblTime.Content = "Zeit: 0";
                Points = 0;
                Turns = 0;
            }
            catch (FormatException) // wird durch int.Parse geworfen
            {
                MessageBox.Show("Eingabe fehlerhaft", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentOutOfRangeException) // wird durch createGame geworfen
            {
                MessageBox.Show("Ungerade Anzahl an Feldern ist nicht erlaubt", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnField_Click(object sender, RoutedEventArgs e)
        {
            if (!Timer.IsEnabled)
            {
                gameStart = DateTime.Now;
                Timer.Start();
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
                        Timer.Stop();
                        // datenbank füllen
                        highScore.AddEntry(Spielfeld.Children.Count, (DateTime.Now - gameStart).TotalSeconds, "Name");

                        // statistik laden und win-screen anzeigen
                        DataGrid dg = new();
                        dg.ItemsSource = highScore.ReadHighscore(Spielfeld.Children.Count);
                        Spielfeld.Clear();
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
