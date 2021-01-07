using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SQLite;
namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MediaPlayer[] mediaPlayers;
        readonly Dictionary<int,string[]> soundSets;
        readonly List<Button> padButtons;


        public MainWindow()
        {
            InitializeComponent(); //lädt die XAML datei zu dieser Klasse
            mediaPlayers = new MediaPlayer[9]; // array erstellen damit dort MediaPlayer reinkönnen
            padButtons = new List<Button>(9);

            // das array aus MediaPlayer wird befüllt
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter] = new MediaPlayer();
            }

            soundSets = new Dictionary<int, string[]>();// Dictionary erstellen damit wir soundsets lagern können

            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = Directory.GetParent(Environment.CommandLine).FullName + "/SoundSets/SoundSets.db";
            builder.Version = 3;

            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "select id, name from soundsets;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        soundSets.Add(reader.GetInt32(0), new string[9]);
                        // für jedes set einen radiobutton erstellen
                        var temp = new RadioButton // neuen RadioButton erstellen
                        {
                            Content = reader.GetString(1),
                            GroupName = "soundSetGroup", // Gruppe nicht vergessen damit sie auch zusammen agieren
                            Name = "Set" + reader.GetInt32(0)
                        };
                        temp.Click += rbSoundSet_Click;
                        temp.IsChecked = reader.GetInt32(0) == 0; // radiobutton 0 anwählen, alle anderen bleiben aus
                        radioContainer.Children.Add(temp); // radiobutton in das stackpanel einsortieren

                    }
                }

                foreach (var set in soundSets)
                {
                    command.CommandText = "select FileName from sounds where soundsetid = @id";
                    command.Parameters.Add(new SQLiteParameter("id", set.Key));
                    using var reader = command.ExecuteReader(); // dieses using gilt bis zum ende des scopes der foreach
                    int soundcounter = 0;
                    while (reader.Read() && soundcounter < 9)
                        soundSets[set.Key][soundcounter++] = reader.GetString(0);
                }
            }

            // die 9 button erstellen
            Grid contentGrid = Content as Grid;
            int buttonCounter = 0;
            for (int rows = 2; rows < 5; rows++)
            {
                for (int cols = 0; cols < 3; cols++)
                {
                    var temp = new Button
                    {
                        Name = "btnPlay" + buttonCounter++,
                        Style = FindResource("PadButtons") as Style
                    };
                    Grid.SetRow(temp, rows); // Button in Zeile sortieren
                    Grid.SetColumn(temp, cols); // Button in Spalte sortieren
                    contentGrid.Children.Add(temp); // button als unterobjekt des Grid eintragen
                    padButtons.Add(temp);
                }
            }
            loadSoundSet(0); // lädt das erste soundset

            // Wenn eine wav datei bis zu ende abgespielt wurde wird der entsprechende button wieder grau
            mediaPlayers[0].MediaEnded += (o, e) => padButtons[0].Background = Brushes.LightGray;
            mediaPlayers[1].MediaEnded += (o, e) => padButtons[1].Background = Brushes.LightGray;
            mediaPlayers[2].MediaEnded += (o, e) => padButtons[2].Background = Brushes.LightGray;
            mediaPlayers[3].MediaEnded += (o, e) => padButtons[3].Background = Brushes.LightGray;
            mediaPlayers[4].MediaEnded += (o, e) => padButtons[4].Background = Brushes.LightGray;
            mediaPlayers[5].MediaEnded += (o, e) => padButtons[5].Background = Brushes.LightGray;
            mediaPlayers[6].MediaEnded += (o, e) => padButtons[6].Background = Brushes.LightGray;
            mediaPlayers[7].MediaEnded += (o, e) => padButtons[7].Background = Brushes.LightGray;
            mediaPlayers[8].MediaEnded += (o, e) => padButtons[8].Background = Brushes.LightGray;
        }

        /// <summary>
        /// Loads the specified sound set into the MediaPlayer array.
        /// </summary>
        /// <param name="SetId">ID number of the set</param>
        private void loadSoundSet(int SetId)
        {
            if (SetId < 0 && SetId >= soundSets.Count) // testen ob die übergebene Zahl auch möglich ist
                throw new IndexOutOfRangeException();

            // alle mediaplayer mit den dateinamen füllen
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
                mediaPlayers[counter].Open(new Uri(Directory.GetParent(Environment.CommandLine).FullName + soundSets[SetId][counter]));

            // falls mitten im abspielen einer wav datei das soundset gewechselt wird muss auch jeder button
            // seine originalfarbe zurückerhalten
            for (int counter = 0; counter < (Content as Grid).Children.Count; counter++) // alle elemente die im Grid eingetragen sind durchgehen
            {
                if (((Content as Grid).Children[counter] as Button) != null && //testen ob das unterobjekt des grid ein button ist
                    ((Content as Grid).Children[counter] as Button).Name.Contains("btnPlay")) // testen ob der button auch den richtigen namen hat
                {
                    ((Content as Grid).Children[counter] as Button).Background = Brushes.LightGray;
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            (sender as Button).Background = Brushes.DarkGreen;
            // der name des buttons entscheidet welcher mediaplayer gestartet wird
            int id = int.Parse((sender as Button).Name[7..]); // ab dem 7ten zeichen ausschneiden, das ergebnis in einen Integer umwandeln.
            mediaPlayers[id].Position = TimeSpan.Zero;// lied wieder zum anfang zurückspulen
            mediaPlayers[id].Play(); // lied starten
        }

        private void rbSoundSet_Click(object sender, RoutedEventArgs e)
        {
            loadSoundSet(int.Parse((sender as RadioButton).Name[3..]));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in mediaPlayers) // alle mediaplayer anhalten und zurückspulen
            {
                item.Pause();
                item.Position = TimeSpan.Zero;
            }

            foreach (var item in padButtons) // alle button auf grau zurücksetzen
            {
                item.Background = Brushes.LightGray;
            }
        }
    }
}
