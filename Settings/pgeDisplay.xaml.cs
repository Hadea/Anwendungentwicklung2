using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Settings
{
    /// <summary>
    /// Interaction logic for pgeAnzeige.xaml
    /// </summary>
    public partial class pgeDisplay : Page
    {
        public pgeDisplay()
        {
            InitializeComponent();
        }

        private void cbNightMode_Click(object sender, RoutedEventArgs e)
        {
            lblColorProfile.Content = cbNightMode.IsChecked.Value ? "Ist an" : "ist aus";
        }
    }
}
