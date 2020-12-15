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
    /// Interaction logic for pgeSelections.xaml
    /// </summary>
    public partial class pgeSelections : Page
    {
        int activeOptions;
        int clickCounter;
        public pgeSelections()
        {
            InitializeComponent();
        }

        private void addOne(object sender, RoutedEventArgs e)
        {
            activeOptions++;
            lblActiveoptions.Content = "Anzahl der gesetzten Checkboxen: " + activeOptions;
        }

        private void subOne(object sender, RoutedEventArgs e)
        {
            activeOptions--;
            lblActiveoptions.Content = "Anzahl der gesetzten Checkboxen: " + activeOptions;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            clickCounter++;
            lblSelectionCount.Content = "Anzahl an Clicks: " + clickCounter;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            lblSingleSelectionValue.Content = ((RadioButton)sender).Content;
        }
    }
}
