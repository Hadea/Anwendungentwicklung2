using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeText.xaml
    /// </summary>
    public partial class pgeText : Page
    {
        public pgeText()
        {
            InitializeComponent();// lädt die XAML datei zu dieser Klasse
        }

        /// <summary>
        /// Indicates that a TextBox has changed its content
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void tbEditEvent_KeyUp(object sender, KeyEventArgs e)
        {
            lblEditEvent.Content = "Verändert";
            lblEditEvent.Foreground = Brushes.Red;
        }

        /// <summary>
        /// Indicates if the password Hallo was correctly entered
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void pbValidation_KeyUp(object sender, KeyEventArgs e)
        {
            lblPassValidation.Content = pbValidation.Password == "Hallo" ? "Hat geklappt!":"Falsch!";
        }
    }
}
