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
    /// Interaction logic for pgeImage.xaml
    /// </summary>
    public partial class pgeImage : Page
    {
        public pgeImage()
        {
            InitializeComponent();
        }

        private void btnVisibility_Click(object sender, RoutedEventArgs e)
        {
            imgVisibility.Visibility = (imgVisibility.Visibility) switch
            {
                Visibility.Visible => Visibility.Hidden,
                Visibility.Hidden => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        private void btnScaling_Click(object sender, RoutedEventArgs e)
        {
            imgScaling.Stretch = (imgScaling.Stretch) switch
            {
                Stretch.None => Stretch.Uniform,
                Stretch.Uniform => Stretch.UniformToFill,
                Stretch.UniformToFill => Stretch.Fill,
                _ => Stretch.None                
            };
        }
    }
}
