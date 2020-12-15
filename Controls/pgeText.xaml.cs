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

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeText.xaml
    /// </summary>
    public partial class pgeText : Page
    {
        public pgeText()
        {
            InitializeComponent();
        }

        private void tbEditEvent_KeyUp(object sender, KeyEventArgs e)
        {
            lblEditEvent.Content = "Verändert";
            lblEditEvent.Foreground = Brushes.Red;
        }

        private void pbValidation_KeyUp(object sender, KeyEventArgs e)
        {
            lblPassValidation.Content = pbValidation.Password == "Hallo" ? "Hat geklappt!":"Falsch!";
        }
    }
}
