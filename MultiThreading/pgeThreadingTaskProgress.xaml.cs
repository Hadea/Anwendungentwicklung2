using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingTaskProgress.xaml
    /// </summary>
    public partial class pgeThreadingTaskProgress : Page
    {
        public pgeThreadingTaskProgress()
        {
            InitializeComponent();
            progressCom = new Progress<int>(refreshProgressBar);
        }

        Task<int> worker;
        Progress<int> progressCom;
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tbOut.Background = Brushes.Blue;
            refreshProgressBar(0);
            worker = new Task<int>(() => { return waitAndColor(progressCom); });
            worker.Start();
            await Task.WhenAll(worker);
            tbOut.Background = Brushes.Green;
            tbOut.Text = worker.Result.ToString();
        }

        private void refreshProgressBar(int reportedProgress)
        {
            pbProgress.Value = reportedProgress;
        }

        private int waitAndColor(IProgress<int> progress)
        {
            int counter = 0;
            while (counter < 1000_000_000)
            {
                counter++;
                if (counter % 1000_000 == 0)
                {
                    progress.Report(counter / 1000_000);
                }
            }
            return counter;
        }
    }
}
