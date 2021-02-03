using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingProgressCancelV2.xaml
    /// </summary>
    public partial class pgeThreadingProgressCancelV2 : Page, INotifyPropertyChanged
    {
        CancellationTokenSource cancelSource;
        int threadCount = 4;
        public ICommand Command_Start { get; init; }

        internal void StopThreads()
        {
            cancelSource.Cancel();
        }

        public ICommand Command_Stop { get; init; }

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

        public int ThreadCount { get => threadCount; set => threadCount = value; }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                    (Command_Start as StartCommand).RaiseCanExecuteChanged();
                }
            }
        }
        private bool _isRunning;

        public pgeThreadingProgressCancelV2()
        {
            InitializeComponent();
            IsRunning = false;
            Command_Start = new StartCommand(this);
            Command_Stop = new StopCommand(this);
            DataContext = this;
        }
        
        public async void Start()
        {
            // UI vorbereiten
            spProgress.Children.Clear();
            Sum = 0;
            Avg = 0;
            int freeProgressID = 0;
            ProgressReporter[] progressReporters = new ProgressReporter[threadCount * 3];

            for (int counter = 0; counter < progressReporters.Length; counter++)
            {
                progressReporters[counter] = new();
                spProgress.Children.Add(progressReporters[counter].ReferencedBar);
            }

            // array erstellen und abschnitte abstecken
            byte[] dataArray = new byte[1_000_000_000];
            int segmentLength = dataArray.Length / threadCount;
            int segmentLengthFirst = dataArray.Length - segmentLength * threadCount + segmentLength;

            // taskcontainer vorbereiten
            cancelSource = new(); // abbruch ermöglichen
            List<Task> fillTasks = new();
            List<Task<long>> sumTasks = new();
            List<Task<byte>> avgTasks = new();

            // array füllen
            {
                var data = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst);
                fillTasks.Add(new Task(() => createArray(data, progressReporters[freeProgressID++].Progress,
                    cancelSource.Token), cancelSource.Token));
            }
            for (int count = 0; count < threadCount - 1; count++)
            {
                var data = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * count, segmentLength);
                fillTasks.Add(new Task(() => createArray(data, progressReporters[freeProgressID++].Progress,
                    cancelSource.Token), cancelSource.Token));
            }

            foreach (var item in fillTasks) item.Start(); // alle füllthreads starten
            await Task.WhenAll(fillTasks); // die Methode btnStart_Click wird pausiert bis alle threads fertig sind
            if (cancelSource.IsCancellationRequested) { IsRunning = false; return; }

            // summe berechnen
            {
                var data = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst);
                sumTasks.Add(new Task<long>(() => sumArray(data, progressReporters[freeProgressID++].Progress,
                        cancelSource.Token), cancelSource.Token));
            }
            for (int count = 0; count < threadCount - 1; count++)
            {

                var data = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * count, segmentLength);
                sumTasks.Add(new Task<long>(() => sumArray(data, progressReporters[freeProgressID++].Progress,
                    cancelSource.Token), cancelSource.Token));
            }
            foreach (var item in sumTasks) item.Start();
            await Task.WhenAll(sumTasks);
            long buffer = 0;
            sumTasks.ForEach((item) => buffer += item.Result);
            Sum = buffer;

            if (cancelSource.IsCancellationRequested) { IsRunning = false; return;}

            // Durchschnitt berechnen
            {
                var data = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst);
                avgTasks.Add(new Task<byte>(() => avgArray(data, progressReporters[freeProgressID++].Progress,
                    cancelSource.Token), cancelSource.Token));
            }
            for (int count = 0; count < threadCount - 1; count++)
            {
                var data = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * count, segmentLength);
                avgTasks.Add(new Task<byte>(() => avgArray(data, progressReporters[freeProgressID++].Progress,
                    cancelSource.Token), cancelSource.Token));
            }

            foreach (var item in avgTasks) item.Start();
            await Task.WhenAll(avgTasks);

            buffer = 0;
            avgTasks.ForEach((item) => buffer += item.Result);
            Avg = (byte)(buffer / avgTasks.Count);
            IsRunning = false;
        }

        static void createArray(ArraySegment<byte> data, IProgress<byte> Progress, CancellationToken Token)
        {
            Random rndGen = new();
            byte progressPercent = 0;
            for (int position = 0; position < data.Count; position++)
            {
                data[position] = (byte)rndGen.Next(256);
                if (position % (data.Count / 100) == 0)
                {
                    Progress.Report(++progressPercent);
                    if (Token.IsCancellationRequested) break;
                }
            }
        }

        static long sumArray(ArraySegment<byte> Data, IProgress<byte> Progress, CancellationToken Token)
        {
            long sum = 0;
            byte progressPercent = 0;
            for (int position = 0; position < Data.Count; position++)
            {
                sum += Data[position];
                if (position % (Data.Count / 100) == 0) // nur 100 mal pro gesamtdurchlauf aktualisieren
                {
                    Progress.Report(++progressPercent);

                    // da auch die abfrage nach dem canceltoken etwas zeit kostet wird das zusammen mit dem fortschritt erledigt
                    if (Token.IsCancellationRequested) break;
                }
            }
            return sum;
        }

        static byte avgArray(ArraySegment<byte> Data, IProgress<byte> Progress, CancellationToken Token)
        {
            long sum = 0;
            byte progressPercent = 0;
            int position = 0;
            for (; position < Data.Count; position++)
            {
                sum += Data[position];
                if (position % (Data.Count / 100) == 0) // nur 100 mal pro gesamtdurchlauf aktualisieren
                {
                    Progress.Report(++progressPercent);

                    // da auch die abfrage nach dem canceltoken etwas zeit kostet wird das zusammen mit dem fortschritt erledigt
                    if (Token.IsCancellationRequested) break;
                }
            }
            return (byte)(sum / ++position);
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
