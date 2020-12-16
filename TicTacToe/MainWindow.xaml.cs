using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameLogic logic;
        Button[,] buttonArray; // enthält alle buttons damit wir schnell auf sie zugreifen können und auch foreach funktioniert

        /// <summary>
        /// Standard Construktor, prepares a new game.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();// läd die XAML datei und erstellt die hinterlegten komponenten. Code immer erst danach einfügen.
            logic = new(); // Die Spiellogik wird erstellt

            // alle buttons welche als Spielfeld verwendet werden werden zusätzlich noch in einem Array
            // abgelegt damit wir sie bequem mit schleifen und koordinaten ansprechen können
            buttonArray = new Button[3, 3];
            buttonArray[0, 0] = btnField11;
            buttonArray[0, 1] = btnField21;
            buttonArray[0, 2] = btnField31;
            buttonArray[1, 0] = btnField12;
            buttonArray[1, 1] = btnField22;
            buttonArray[1, 2] = btnField32;
            buttonArray[2, 0] = btnField13;
            buttonArray[2, 1] = btnField23;
            buttonArray[2, 2] = btnField33;
            lblMessage.Content = $"Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} ist am Zug";
        }

        /// <summary>
        /// Resets the logic and the playing field
        /// </summary>
        /// <param name="sender">Reset Button reference, currently unused</param>
        /// <param name="e">Arguments, currently unused</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            logic.Reset();// logik wird wieder auf den anfangszustand gesetzt

            // alle button werden reaktiviert und der text entfernt
            foreach (var item in buttonArray)
            {
                item.Content = "";
                item.IsEnabled = true;
            }

            lblMessage.Content = $"Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} ist am Zug";
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
                }
            }
        }

        /// <summary>
        /// GUI logic for a turn
        /// </summary>
        /// <param name="Result">Result of Turn()</param>
        /// <returns>If a button should be active</returns>
        private bool evaluateTurn(TurnResult Result)
        {
            switch (Result) // wertet den Parameter Result aus
            {
                case TurnResult.Valid: // wenn der zug gültig war
                    redrawField(); // spielfeld aktualisieren
                    return false; // rückgabe das der Button inaktiv werden soll
                case TurnResult.Win: // wenn der zug zum sieg geführt hat
                    redrawField(); // spielfeld aktualisieren
                    lblMessage.Content = $"Sieg! Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} hat gewonnen"; // nachricht ausgeben das der aktuelle spieler gewonnen hat

                    // alle button deaktivieren, da das Spiel vorbei ist
                    foreach (var item in buttonArray)
                    {
                        item.IsEnabled = false;
                    }
                    return false;
                case TurnResult.Tie: // wenn der zug zu einem unentschieden geführt hat
                    redrawField(); // spielfeld aktualisieren
                    lblMessage.Content = "Unentschieden!"; // Spieler benachrichtigen
                    return false; // rückgabe das der button der zum unentschieden geführt hat deaktiviert werden soll
                default: // wenn der zug ungültig war (sollte nicht auftreten)
                    lblMessage.Content = "Das war nix";
                    return true; // button darf aktiv bleiben
            }
        }

        // Methoden für die 9 Spielfeld-Buttons
        // Jeder button löst mit seinen koordinaten einen Zug aus, und reicht das
        // ergebnis des zuges an das evaluateTurn weiter
        // evaluateTurn gibt als Rückgabe ob der button sich deaktivieren soll oder nicht
        private void btnField11_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 0)));
        private void btnField21_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 0)));
        private void btnField31_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 0)));
        private void btnField12_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 1)));
        private void btnField22_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 1)));
        private void btnField32_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 1)));
        private void btnField13_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 2)));
        private void btnField23_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 2)));
        private void btnField33_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 2)));

    }
}
