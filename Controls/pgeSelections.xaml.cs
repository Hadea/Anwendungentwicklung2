using System.Windows;
using System.Windows.Controls;

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeSelections.xaml
    /// </summary>
    public partial class pgeSelections : Page
    {
        int activeOptions; // zähler für die aktiven Checkboxen
        int clickCounter; // zähler für die Interaktionen mit Checkboxen
        public pgeSelections()
        {
            InitializeComponent();// Läd die XAML datei zu dieser Klasse
        }
        /// <summary>
        /// Increases the number of active Boxes and displays the current value.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void addOne(object sender, RoutedEventArgs e)
        {
            activeOptions++;
            lblActiveoptions.Content = "Anzahl der gesetzten Checkboxen: " + activeOptions;
        }

        /// <summary>
        /// Decreases the number of active Boxes and displays the current value.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void subOne(object sender, RoutedEventArgs e)
        {
            activeOptions--;
            lblActiveoptions.Content = "Anzahl der gesetzten Checkboxen: " + activeOptions;
        }

        /// <summary>
        /// Increases the number of interactions and displays the current value.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            clickCounter++;
            lblSelectionCount.Content = "Anzahl an Clicks: " + clickCounter;
        }

        /// <summary>
        /// Fills a label with the Content of the currently selected radio button.
        /// </summary>
        /// <param name="sender">RadioButton wicht will provide the content for the label</param>
        /// <param name="e">unused</param>
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            // sender enthält die Referenz zu dem Objekt welches diese Methode gestartet
            // hat. Da wir auf eine Variable des RadioButton zugreifen wollen und auch wissen
            // das diese Methode nur von einem RadioButton ausgelöst wird können wir
            // sender uminterpretieren lassen zu einem RadioButton
            // ((RadioButton)sender).Content
            //      versucht das object sender als RadioButton zu interpretieren und wirft
            //      eine exception wenn dies nicht gelingt
            // (sender as RadioButton).Content
            //      versucht das objekt sender als RadioButton zu interpretieren, sollte dies
            //      nicht gelingen entsteht hier ein null.
            lblSingleSelectionValue.Content = (sender as RadioButton).Content;
        }
    }
}
