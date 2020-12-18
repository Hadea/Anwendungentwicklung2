using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MediaPlayer[] mediaPlayers;
        readonly List<string[]> soundSets;

        public MainWindow()
        {
            InitializeComponent(); //lädt die XAML datei zu dieser Klasse
            mediaPlayers = new MediaPlayer[9]; // array erstellen damit dort MediaPlayer reinkönnen

            // das array aus MediaPlayer wird befüllt
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter] = new MediaPlayer();
            }

            soundSets = new List<string[]>();// Liste erstellen damit wir soundsets lagern können
            using (StreamReader reader = new StreamReader("SoundSets/SoundSets.txt")) // datei mit den soundsets laden
            {
                int soundCounter = 0; // zähler um zu wissen ob ein weiteres soundset geladen wird
                string[] soundSet = null; // lagert die referenz zu dem array welches wir befüllen wollen
                string fileName; // wird die gelesene Zeile enthalten
                while ((fileName = reader.ReadLine()) != null) // solange wir noch Zeilen in der Datei haben weiterlesen
                {
                    if (soundCounter == 0)
                    {
                        // immer wenn wir am anfang eines neuen sets sind, array erstellen und in die Liste der sets eintragen
                        soundSet = new string[9];
                        soundSets.Add(soundSet);
                    }
                    soundSet[soundCounter] = fileName; // gelesene Zeile mit dem dateinamen wird in das soundset eingetragen
                    soundCounter++;

                    if (soundCounter >= soundSet.Length) // wenn wir das ende (9) des arrays erreicht haben wird im nächsten durchlauf ein neues erstellt
                    {
                        soundCounter = 0;
                    }
                }
            }

            loadSoundSet(0); // lädt das erste soundset

            // für jedes set einen radiobutton erstellen
            for (int counter = 0; counter < soundSets.Count; counter++)
            {
                var temp = new RadioButton // neuen RadioButton erstellen
                {
                    Content = "Set " + counter,
                    GroupName = "soundSetGroup", // Gruppe nicht vergessen damit sie auch zusammen agieren
                    Name = "Set" + counter
                };
                temp.Click += rbSoundSet_Click;
                temp.IsChecked = counter == 0; // radiobutton 0 anwählen, alle anderen bleiben aus
                radioContainer.Children.Add(temp); // radiobutton in das stackpanel einsortieren
            }

            // die 9 button erstellen
            Grid contentGrid = Content as Grid;
            int buttonCounter = 0;
            List<Button> buttons = new List<Button>(9);
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
                    buttons.Add(temp);
                }
            }

            // Wenn eine wav datei bis zu ende abgespielt wurde wird der entsprechende button wieder grau
            mediaPlayers[0].MediaEnded += (o, e) => buttons[0].Background = Brushes.LightGray;
            mediaPlayers[1].MediaEnded += (o, e) => buttons[1].Background = Brushes.LightGray;
            mediaPlayers[2].MediaEnded += (o, e) => buttons[2].Background = Brushes.LightGray;
            mediaPlayers[3].MediaEnded += (o, e) => buttons[3].Background = Brushes.LightGray;
            mediaPlayers[4].MediaEnded += (o, e) => buttons[4].Background = Brushes.LightGray;
            mediaPlayers[5].MediaEnded += (o, e) => buttons[5].Background = Brushes.LightGray;
            mediaPlayers[6].MediaEnded += (o, e) => buttons[6].Background = Brushes.LightGray;
            mediaPlayers[7].MediaEnded += (o, e) => buttons[7].Background = Brushes.LightGray;
            mediaPlayers[8].MediaEnded += (o, e) => buttons[8].Background = Brushes.LightGray;
        }

        private void loadSoundSet(int SetId)
        {
            // alle mediaplayer mit den dateinamen füllen
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter].Open(new Uri(Directory.GetParent(Environment.CommandLine).FullName + soundSets[SetId][counter]));
            }

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
            int id = int.Parse((sender as Button).Name[7..]);
            mediaPlayers[id].Position = TimeSpan.Zero;
            mediaPlayers[id].Play();
        }

        private void rbSoundSet_Click(object sender, RoutedEventArgs e)
        {
            loadSoundSet(int.Parse((sender as RadioButton).Name[3..]));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in mediaPlayers)
            {
                item.Pause();
                item.Position = TimeSpan.Zero;
            }

            for (int counter = 0; counter < (Content as Grid).Children.Count; counter++)
            {
                if (((Content as Grid).Children[counter] as Button) != null && ((Content as Grid).Children[counter] as Button).Name.Contains("btnPlay"))
                {
                    ((Content as Grid).Children[counter] as Button).Background = Brushes.LightGray;
                }
            }
        }


    }
}
