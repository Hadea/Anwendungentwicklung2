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
    /// Interaction logic for pgeThreadingTaskCancel.xaml
    /// </summary>
    public partial class pgeThreadingTaskCancel : Page
    {
        Task<int> worker;

        CancellationTokenSource cancelTokenSource;
        public pgeThreadingTaskCancel()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            rectOut.Fill = Brushes.Blue;
            cancelTokenSource = new CancellationTokenSource();
            worker = new Task<int>(() => { return waitAndColor(cancelTokenSource.Token); }, cancelTokenSource.Token);
            worker.Start();
            await Task.WhenAll(worker);
            rectOut.Fill = Brushes.Green;
            tblOut.Text = worker.Result.ToString();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            cancelTokenSource.Cancel();
        }

        private int waitAndColor(CancellationToken CancelToken)
        {
            int counter = 0;
            while (counter < 1000000000)
            {
                counter++;

                if (counter % 1000000 == 0 && !CancelToken.IsCancellationRequested)
                {
                    CancelToken.ThrowIfCancellationRequested();
                }
            }
            return counter;
        }
    }
}
