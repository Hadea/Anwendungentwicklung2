using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeWithoutThreading.xaml
    /// </summary>
    public partial class pgeWithoutThreading : Page
    {
        public pgeWithoutThreading()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tblAusgabe.Text = "running";
            tblAusgabe.Text = Wait5SecForString();
        }

        public static string Wait5SecForString()
        {
            Thread.Sleep(5000);
            return "done";
        }
    }
}
