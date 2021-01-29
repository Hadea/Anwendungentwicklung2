using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            tblAusgabe.Text = await Wait5SecForString();
            tblAusgabe.Text = await Wait5SecForString2();
        }

        public static async Task<string> Wait5SecForString()
        {
            await Task.Delay(5000);
            return "Done";
        }
        public static async Task<string> Wait5SecForString2()
        {
            await Task.Delay(5000);
            return "Done 2";
        }
    }
}
