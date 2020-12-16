using System.Windows;

namespace Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();// lädt die XAML zu dieser Klasse
        }

        /// <summary>
        /// Loads display settings into the content frame.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeDisplay());// läd eine Seite in den Frame
        }

        /// <summary>
        /// loads sound settings into the content frame
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnSound_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeSound());// läd eine Seite in den Frame
        }
    }
}
