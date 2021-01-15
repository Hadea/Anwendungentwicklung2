using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Binding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadBindingIntro(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingIntro());
        }

        private void loadBindingIntroExercise(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingIntroExercise());
        }

        private void loadBindingDirection(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingDirection());
        }

        private void loadBindingExternal(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingExternal(this));
        }

        private void loadBindingFormatAndConvert(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingFormatAndConvert());
        }

        private void loadBindingConvertExercise(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingConvertExercise());
        }

        private void loadBindingProperties(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingProperties());
        }

        private void loadBindingCommand(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new BindingCommands());
        }
    }
}
