using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeImage.xaml
    /// </summary>
    public partial class pgeImage : Page
    {
        public pgeImage()
        {
            InitializeComponent();// lädt die XAML datei zu dieser Klasse
        }

        /// <summary>
        /// Cycles visibility of the Image
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnVisibility_Click(object sender, RoutedEventArgs e)
        {
            // die drei möglichen sichtbarkeitseinstellungen werden
            // der reihe nach durchgeschaltet bei jedem Klick auf den button
            imgVisibility.Visibility = (imgVisibility.Visibility) switch
            {
                Visibility.Visible => Visibility.Hidden, // falls vorher Visible drin stand soll nun auf Hidden gesetzt werden
                Visibility.Hidden => Visibility.Collapsed, // falls vorher Hidden drin stand soll nun auf Collapsed gesetzt werden
                _ => Visibility.Visible // in jedem anderen fall (gibt nur noch Collapsed) wird auf Visible gesetzt
            };
        }

        /// <summary>
        /// Cycles scaling of the Image
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void btnScaling_Click(object sender, RoutedEventArgs e)
        {
            imgScaling.Stretch = (imgScaling.Stretch) switch
            {
                Stretch.None => Stretch.Uniform,
                Stretch.Uniform => Stretch.UniformToFill,
                Stretch.UniformToFill => Stretch.Fill,
                _ => Stretch.None                
            };

            lblCurrentStretch.Content = imgScaling.Stretch.ToString();
        }
    }
}
