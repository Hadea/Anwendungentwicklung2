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
    /// Interaction logic for pgeThreadingProgressCancel.xaml
    /// </summary>
    public partial class pgeThreadingProgressCancel : Page
    {
        CancellationTokenSource SumCancelSource;
        readonly Progress<int> SumProgress;
        public pgeThreadingProgressCancel()
        {
            InitializeComponent();
            SumProgress = new(updateProgressBar);
        }

        private void updateProgressBar(int NewValue)
        {
            pbProgress.Value = NewValue;
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            pbProgress.Value = 0;
            SumCancelSource = new CancellationTokenSource();
            long result = await Task<Int64>.Run(() => sumRandomArray(SumProgress, SumCancelSource.Token), SumCancelSource.Token);
            tbResult.Text = result.ToString();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true); GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            SumCancelSource.Cancel();
        }

        private static long sumRandomArray(IProgress<int> sumProgress, CancellationToken token)
        {
            byte[] randomArray = new byte[100_000_000];
            Random rndGen = new();
            int progressPerMille = 0;
            for (int position = 0; position < randomArray.Length; position++)
            {
                randomArray[position] = (byte)rndGen.Next(256);
                if (position % (randomArray.Length / 500) == 0)
                {
                    sumProgress.Report(++progressPerMille);
                    if (token.IsCancellationRequested) return 0;
                }
            }

            long sum = 0;
            for (int position = 0; position < randomArray.Length; position++)
            {
                sum += randomArray[position];
                if (position % (randomArray.Length / 500) == 0)
                {
                    sumProgress.Report(++progressPerMille);
                    if (token.IsCancellationRequested)
                        return sum;
                }
            }
            return sum;
        }
    }
}
