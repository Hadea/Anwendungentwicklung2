using System.Windows;

namespace Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // lädt die XAML zu dieser Klasse
        }
        /// <summary>
        /// Loads pgeText into the Frame
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeText());
            // füllt den Frame mit einer neuen Page
            // der alte inhalt wird in diesem fall dabei Zerstört, da es keine weitere
            // variable (referenz) gibt die auf das Objekt zeigt
        }

        /// <summary>
        /// Loads pgeImage into the Frame
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeImage());
            // füllt den Frame mit einer neuen Page
            // der alte inhalt wird in diesem fall dabei Zerstört, da es keine weitere
            // variable (referenz) gibt die auf das Objekt zeigt
        }

        /// <summary>
        /// Loads pgeSelections into the Frame
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnSelections_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeSelections());
            // füllt den Frame mit einer neuen Page
            // der alte inhalt wird in diesem fall dabei Zerstört, da es keine weitere
            // variable (referenz) gibt die auf das Objekt zeigt
        }
    }
}
