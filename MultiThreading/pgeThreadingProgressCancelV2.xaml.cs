using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingProgressCancelV2.xaml
    /// </summary>
    public partial class pgeThreadingProgressCancelV2 : Page, INotifyPropertyChanged
    {
        CancellationTokenSource cancelSource;
        const int threadCount = 4;
        readonly List<Task<long>> sumTasks = new();
        readonly List<Task<byte>> avgTasks = new();

        private long _sum = 0;

        public long Sum
        {
            get { return _sum; }
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
                }
            }
        }

        private byte _avg = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public byte Avg
        {
            get { return _avg; }
            set
            {
                if (_avg != value)
                {
                    _avg = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Avg)));
                }
            }
        }

        public pgeThreadingProgressCancelV2() => InitializeComponent();
        private void btnStop_Click(object sender, RoutedEventArgs e) => cancelSource.Cancel();

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            cancelSource = new();
            spProgress.Children.Clear();

            int freeProgressID = 0;

            ProgressReporter[] progressReporters = new ProgressReporter[threadCount * 2 + 1];

            for (int counter = 0; counter < progressReporters.Length; counter++)
            {
                progressReporters[counter] = new();
                spProgress.Children.Add(progressReporters[counter].ReferencedBar);
            }


            byte[] dataArray = await Task.Run(() => createArray(1_000_000_000, progressReporters[freeProgressID++].Progress, cancelSource.Token), cancelSource.Token);
            if (cancelSource.IsCancellationRequested) return;

            int segmentLength = dataArray.Length / threadCount;
            int segmentLengthFirst = dataArray.Length - segmentLength * threadCount + segmentLength;

            sumTasks.Add(new Task<long>(() => sumArray(
                    new ThreadDataContainer { ThreadID = 0,
                        DataArray = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst) },
                    progressReporters[freeProgressID++].Progress,
                    cancelSource.Token),
                cancelSource.Token));
            for (int count = 0; count < threadCount-1; count++)
            {
                ThreadDataContainer data;
                data.DataArray = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * count, segmentLength);
                data.ThreadID = count + 1;
                sumTasks.Add(new Task<long>(() => sumArray(data, progressReporters[freeProgressID++].Progress, cancelSource.Token), cancelSource.Token));
            }

            avgTasks.Add(new Task<byte>(() => avgArray(new ThreadDataContainer { ThreadID = threadCount + 1, DataArray = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst) }, progressReporters[freeProgressID++].Progress, cancelSource.Token), cancelSource.Token));
            for (int count = 0; count < threadCount-1; count++)
            {
                ThreadDataContainer data;
                data.DataArray = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * count, segmentLength);
                data.ThreadID = count + 1;
                avgTasks.Add(new Task<byte>(() => avgArray(data, progressReporters[freeProgressID++].Progress, cancelSource.Token), cancelSource.Token));
            }

            foreach (var item in sumTasks) item.Start();
            foreach (var item in avgTasks) item.Start();

            await Task.WhenAll(sumTasks);
            await Task.WhenAll(avgTasks);

            long buffer = 0;
            sumTasks.ForEach((item) => buffer += item.Result);
            Sum = buffer;
            buffer = 0;
            avgTasks.ForEach((item) => buffer += item.Result);
            Avg = (byte)(buffer / avgTasks.Count);
        }

        static byte[] createArray(int Length, IProgress<byte> Progress, CancellationToken Token)
        {
            byte[] randomArray = new byte[Length];
            Random rndGen = new();
            byte progressPercent = 0;
            for (int position = 0; position < randomArray.Length; position++)
            {
                randomArray[position] = (byte)rndGen.Next(256);
                if (position % (randomArray.Length / 100) == 0)
                {
                    Progress.Report(++progressPercent);
                    if (Token.IsCancellationRequested) break;
                }
            }
            return randomArray;
        }

        static long sumArray(ThreadDataContainer Data, IProgress<byte> Progress, CancellationToken Token)
        {
            long sum = 0;
            byte progressPercent = 0;
            for (int position = 0; position < Data.DataArray.Count; position++)
            {
                sum += Data.DataArray[position];
                if (position % (Data.DataArray.Count / 100) == 0) // nur 300 mal pro gesamtdurchlauf aktualisieren
                {
                    Progress.Report(++progressPercent);

                    // da auch die abfrage nach dem canceltoken etwas zeit kostet wird das zeitgleich mit dem fortschritt erledigt
                    if (Token.IsCancellationRequested) break;
                }
            }
            return sum;
        }

        static byte avgArray(ThreadDataContainer Data, IProgress<byte> Progress, CancellationToken Token)
        {
            long sum = 0;
            byte progressPercent = 0;
            int position = 0;
            for (; position < Data.DataArray.Count; position++)
            {
                sum += Data.DataArray[position];
                if (position % (Data.DataArray.Count / 100) == 0) // nur 300 mal pro gesamtdurchlauf aktualisieren
                {
                    Progress.Report(++progressPercent);

                    // da auch die abfrage nach dem canceltoken etwas zeit kostet wird das zeitgleich mit dem fortschritt erledigt
                    if (Token.IsCancellationRequested) break;
                }
            }
            return (byte)(sum / ++position);
        }

        struct ThreadDataContainer
        {
            public int ThreadID;
            public ArraySegment<byte> DataArray;
        }

        class ProgressReporter
        {
            public readonly ProgressBar ReferencedBar;
            public readonly Progress<byte> Progress;
            public ProgressReporter()
            {
                ReferencedBar = new ProgressBar();
                Progress = new(Report);
            }
            public void Report(byte NewValue)
            {
                ReferencedBar.Value = NewValue;
            }

        }

    }
}
