using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingProgressCancelV2.xaml
    /// </summary>
    public partial class pgeThreadingProgressCancelV2 : Page, INotifyPropertyChanged
    {
        private CancellationTokenSource cancelSource;
        public event PropertyChangedEventHandler PropertyChanged;


        public ICommand Command_Start { get; init; }
        public ICommand Command_Stop { get; init; }
        public ICommand Command_Clear { get; init; }


        public List<int> WorkloadList { get; set; } = new List<int> { 1_000_000_000, 500_000_000, 100_000, 1_000 };
        public int SelectedWorkload { get; set; } = 500_000_000;

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


        private long _sum = 0;

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
        private byte _avg = 0;

        public int ThreadCount { get => threadCount; set => threadCount = value; }
        private int threadCount = 2;

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotRunning)));
                    (Command_Start as StartCommand).RaiseCanExecuteChanged();
                    (Command_Stop as StopCommand).RaiseCanExecuteChanged();
                    (Command_Clear as ClearCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public bool IsNotRunning
        {
            get => !_isRunning;
            set
            {
                if (_isRunning == value)
                {
                    _isRunning = !value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotRunning)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                    (Command_Start as StartCommand).RaiseCanExecuteChanged();
                    (Command_Stop as StopCommand).RaiseCanExecuteChanged();
                    (Command_Stop as ClearCommand).RaiseCanExecuteChanged();
                }
            }
        }
        private bool _isRunning;

        public int WorkloadSplit { get; set; } = 20;

        public void StopThreads() => cancelSource.Cancel();
        internal void Clear()
        {
            spProgress.Children.Clear();
            Sum = 0;
            Avg = 0;
        }

        public pgeThreadingProgressCancelV2()
        {
            InitializeComponent();
            IsRunning = false;
            Command_Start = new StartCommand(this);
            Command_Stop = new StopCommand(this);
            Command_Clear = new ClearCommand(this);
            DataContext = this;
        }

        public async void StartUserSplit()
        {
            IsRunning = true;
            Clear();
            // leeres array erstellen
            byte[] dataArray = new byte[SelectedWorkload];
            int usedThreads = Environment.ProcessorCount;

            // aufteilen des arrays
            int segmentLength = dataArray.Length / WorkloadSplit;
            int segmentLengthFirst = dataArray.Length - segmentLength * WorkloadSplit + segmentLength;

            cancelSource = new();

            // creation
            List<ProgressReporter> creationReporter = new(WorkloadSplit);
            {
                ProgressReporter chunk = new()
                {
                    WorkSegment = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst)
                };
                chunk.ReferencedBar.Style = (Style)FindResource("stylePBWork");
                spProgress.Children.Add(chunk.ReferencedBar);
                creationReporter.Add(chunk);
            }
            for (int counter = 0; counter < WorkloadSplit - 1; counter++)
            {
                ProgressReporter chunk = new()
                {
                    WorkSegment = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * counter, segmentLength)
                };
                chunk.ReferencedBar.Style = (Style)FindResource("stylePBWork");
                spProgress.Children.Add(chunk.ReferencedBar);
                creationReporter.Add(chunk);
            }

            spProgress.Children.Add(new Border { Width = spProgress.ActualWidth - 5, Height=2 });
            List<ProgressReporter> WorkReporter = new(WorkloadSplit*2);

            for (int i = 0; i < 2; i++)
            {
                {
                    ProgressReporter chunk = new()
                    {
                        WorkSegment = new ArraySegment<byte>(dataArray, 0, segmentLengthFirst)
                    };
                    chunk.ReferencedBar.Style = (Style)FindResource("stylePBWork");
                    spProgress.Children.Add(chunk.ReferencedBar);
                    WorkReporter.Add(chunk);
                }
                for (int counter = 0; counter < WorkloadSplit - 1; counter++)
                {
                    ProgressReporter chunk = new()
                    {
                        WorkSegment = new ArraySegment<byte>(dataArray, segmentLengthFirst + segmentLength * counter, segmentLength)
                    };
                    chunk.ReferencedBar.Style = (Style)FindResource("stylePBWork");
                    spProgress.Children.Add(chunk.ReferencedBar);
                    WorkReporter.Add(chunk);
                }
            }

            //TODO: Options
            //Hack: reimplement ForEachAsync / split functions
            await Task.Run(() => Parallel.ForEach(creationReporter, (data) => createArray(data.WorkSegment, data.Progress, cancelSource.Token)));
            if (cancelSource.IsCancellationRequested) { IsRunning = false; return; }
            await Task.Run(() => Parallel.ForEach(WorkReporter, (data) => sumArray(data.WorkSegment, data.Progress, cancelSource.Token)));

            IsRunning = false;
        }
        public async void StartThreadSplit()
        {
            IsRunning = true;
            // UI vorbereiten
            Clear();
            int freeProgressID = 0;
            ProgressReporter[] progressReporters = new ProgressReporter[threadCount * 3];

            for (int counter = 0; counter < progressReporters.Length; counter++)
            {
                progressReporters[counter] = new();
                progressReporters[counter].ReferencedBar.Style = (Style)FindResource("stylePBThread");
                spProgress.Children.Add(progressReporters[counter].ReferencedBar);
            }

            // array erstellen und abschnitte abstecken
            byte[] dataArray = new byte[SelectedWorkload];
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

            if (cancelSource.IsCancellationRequested) { IsRunning = false; return; }

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

        ///////////////////   Not object related //////////////////

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

        class ProgressReporter // nested class
        {
            public readonly ProgressBar ReferencedBar;
            public readonly Progress<byte> Progress;
            public ArraySegment<byte> WorkSegment;
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
