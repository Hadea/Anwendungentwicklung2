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
    /// Interaction logic for pgeThreadingTaskProgress.xaml
    /// </summary>
    public partial class pgeThreadingTaskProgress : Page
    {
        public pgeThreadingTaskProgress()
        {
            InitializeComponent();
        }

        Task<int> worker;

        CancellationTokenSource cancelTokenSource;

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tbOut.Background = Brushes.Blue;
            cancelTokenSource = new CancellationTokenSource();
            worker = new Task<int>(() => { return waitAndColor(cancelTokenSource.Token); }, cancelTokenSource.Token);
            worker.Start();
            await Task.WhenAll(worker);
            tbOut.Background = Brushes.Green;
            tbOut.Text = worker.Result.ToString();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }

        private int waitAndColor(CancellationToken CancelToken, IProgress<int> progress)
        {
            int counter = 0;
            while (counter < 1000000000)
            {
                counter++;
                progress.Report(counter / 100000);
                if (counter % 1000000 == 0 && !CancelToken.IsCancellationRequested)
                {
                    CancelToken.ThrowIfCancellationRequested();
                }
            }
            return counter;
        }
    }
}
