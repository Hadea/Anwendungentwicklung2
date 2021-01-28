using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingAsync.xaml
    /// </summary>
    public partial class pgeThreadingAsync : Page
    {
        public pgeThreadingAsync()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tblAusgabe.Text = "running";
            tblAusgabe.Text = await Task.Run(() => Wait5SecForString());
        }

        public static string Wait5SecForString()
        {
            Thread.Sleep(5000);
            return "Done";
        }
    }
}
