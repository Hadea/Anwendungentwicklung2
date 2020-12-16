using System.Windows;

namespace WPFEinfuehrung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int clickCounter;

        public MainWindow()
        {
            InitializeComponent(); // lädt die XAML datei welche zu dieser Klasse gehört
            clickCounter = 0;
        }

        /// <summary>
        /// Counts Clicks and displays the current count in a label
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnGreeting_Click(object sender, RoutedEventArgs e)
        {
            clickCounter++;

            // $ aktiviert den modus das variablen mitten im string wie platzhalter
            // verwendet werden können (clickCounter). die 3 besagt das immer mindestens
            // 3 zeichen eingefügt werden. Sollte clickCounter weniger buchstaben verwenden
            // wird dieser rechtsbündig geschrieben und links mit leerzeichen aufgefüllt
            lblGreeting.Text = $"Button wurde {clickCounter, 3} mal geklickt :D";
        }
    }
}
