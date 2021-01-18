using Microsoft.Win32;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using TicTacToeLogic;
using System.ComponentModel;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        readonly GameLogic logic;
        readonly Button[,] buttonArray; // enthält alle buttons damit wir schnell auf sie zugreifen können und auch foreach funktioniert

        readonly MediaPlayer soundMusic;

        private string message;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private bool musicPlaying;

        public bool Music
        {
            get { return musicPlaying; }
            set
            {
                if (musicPlaying != value)
                {
                    musicPlaying = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Music)));
                    if (musicPlaying)
                        soundMusic.Play();
                    else
                    {
                        soundMusic.Pause();
                        soundMusic.Position = TimeSpan.Zero;
                    }
                }
            }
        }


        /// <summary>
        /// Standard Construktor, prepares a new game.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();// läd die XAML datei und erstellt die hinterlegten komponenten. Code immer erst danach einfügen.
            logic = new(); // Die Spiellogik wird erstellt
            DataContext = this;

            // alle buttons welche als Spielfeld verwendet werden werden zusätzlich noch in einem Array
            // abgelegt damit wir sie bequem mit schleifen und koordinaten ansprechen können
            buttonArray = new Button[3, 3];
            buttonArray[0, 0] = grdFieldButtons.Children[0] as Button;
            buttonArray[0, 1] = grdFieldButtons.Children[1] as Button;
            buttonArray[0, 2] = grdFieldButtons.Children[2] as Button;
            buttonArray[1, 0] = grdFieldButtons.Children[3] as Button;
            buttonArray[1, 1] = grdFieldButtons.Children[4] as Button;
            buttonArray[1, 2] = grdFieldButtons.Children[5] as Button;
            buttonArray[2, 0] = grdFieldButtons.Children[6] as Button;
            buttonArray[2, 1] = grdFieldButtons.Children[7] as Button;
            buttonArray[2, 2] = grdFieldButtons.Children[8] as Button;
            Message = $"Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} ist am Zug";
            soundMusic = new();
            soundMusic.Open(new Uri(Directory.GetParent(Environment.CommandLine) + @"\ShaolinDub-HarpDubz.mp3", UriKind.Absolute));

        }

        /// <summary>
        /// Reads the playing field and adjusts the buttons accordingly
        /// </summary>
        private void redrawField()
        {
            var board = logic.GetBoard();// Board aus der Logik lesen

            // Alle button durchgehen und entsprechend des Boards mit X, O oder leerem text füllen
            for (int y = 0; y < buttonArray.GetLength(0); y++)
            {
                for (int x = 0; x < buttonArray.GetLength(1); x++)
                {
                    buttonArray[y, x].Content = board[y, x] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
                    //buttonArray[y, x].IsEnabled = board[y, x] switch { FieldState.X => false, FieldState.O => false, _ => true };
                }
            }
        }

        /// <summary>
        /// GUI logic for a turn
        /// </summary>
        /// <param name="Result">Result of Turn()</param>
        /// <returns>If a button should be active</returns>
        private void evaluateTurn(TurnResult Result)
        {
            switch (Result) // wertet den Parameter Result aus
            {
                case TurnResult.Valid: // wenn der zug gültig war
                    redrawField(); // spielfeld aktualisieren
                    Message = $"Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} ist am Zug";
                    break;
                case TurnResult.Win: // wenn der zug zum sieg geführt hat
                    redrawField(); // spielfeld aktualisieren
                    Message = $"Sieg! Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} hat gewonnen"; // nachricht ausgeben das der aktuelle spieler gewonnen hat

                    // alle button deaktivieren, da das Spiel vorbei ist
                    foreach (var item in buttonArray)
                        item.IsEnabled = false;
                    break;
                case TurnResult.Tie: // wenn der zug zu einem unentschieden geführt hat
                    redrawField(); // spielfeld aktualisieren
                    Message = "Unentschieden!"; // Spieler benachrichtigen
                    break;
                default: // wenn der zug ungültig war (sollte nicht auftreten)
                    Message = "Das war nix";
                    break;
            }
        }


        private void New_Command(object sender, ExecutedRoutedEventArgs e)
        {
            logic.Reset();// logik wird wieder auf den anfangszustand gesetzt

            // alle button werden reaktiviert und der text entfernt
            foreach (var item in buttonArray)
            {
                item.Content = "";
                //item.IsEnabled = true;
            }

            Message = $"Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} ist am Zug";
        }

        private void Close_Command(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Select_Command(object sender, ExecutedRoutedEventArgs e)
        {
            object[] parameters = e.Parameter as object[];
            byte X = (byte)parameters[0];
            byte Y = (byte)parameters[1];
            evaluateTurn(logic.Turn(new PointB(X, Y)));
        }

        private void Select_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                object[] parameters = e.Parameter as object[];
                byte X = (byte)parameters[0];
                byte Y = (byte)parameters[1];
                e.CanExecute = logic.GetBoard()[Y, X] switch { FieldState.O => false, FieldState.X => false, _ => true };
            }
            else
                e.CanExecute = false;
        }
    }
}
