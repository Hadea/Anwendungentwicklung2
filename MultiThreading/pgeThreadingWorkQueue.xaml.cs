using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingWorkQueue.xaml
    /// </summary>
    public partial class pgeThreadingWorkQueue : Page, INotifyPropertyChanged
    {
        public pgeThreadingWorkQueue()
        {
            InitializeComponent();
            DataContext = this;
            ProgressBars = new();
            Command_Start = new StartWorkQueueCommand(calculate);

        }

        public int WorkPackageNumber { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand Command_Start { get; init; }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
        private string _message;


        public ObservableCollection<ProgressBar> ProgressBars { get; set; }


        private async void calculate()
        {
            DateTime startTime = DateTime.Now;
            Message = "Starte";
            //wpProgress.Children.Clear();
            ProgressBars.Clear();
            // vorbereitungen
            byte[] arrayToWorkOn = new byte[1_000_000_000];

            // arbeit in pakete aufteilen
            Queue<WorkPackage> packages = new();
            int segmentLength = arrayToWorkOn.Length / WorkPackageNumber;
            int segmentLengthRemainder = segmentLength + arrayToWorkOn.Length % WorkPackageNumber;

            packages.Enqueue(new WorkPackage(new ArraySegment<byte>(arrayToWorkOn, 0, segmentLengthRemainder)));
            for (int counter = 0; counter < WorkPackageNumber - 1; counter++)
                packages.Enqueue(new WorkPackage(new ArraySegment<byte>(arrayToWorkOn, segmentLengthRemainder + counter * segmentLength, segmentLength)));


            LinkedList<Task> taskList = new();

            // solange noch Pakete vorhanden sind tasks pakete zuweisen
            while (packages.Count > 0)
            {
                if (taskList.Count < Environment.ProcessorCount - 1)
                {
                    // tasks erstellen
                    WorkPackage myPackage = packages.Dequeue(); // package aus der Queue holen
                    taskList.AddLast(Task.Run(() =>
                    {
                        Random rnd = new Random();
                        for (int counter = 0; counter < myPackage.Segment.Count; counter++)
                            myPackage.Segment[counter] = (byte)rnd.Next(256);
                    }));
                }
                else
                {
                    Task returnedTask = await Task.WhenAny(taskList);
                    // warten bis einer fertig ist und dann recyclen
                    taskList.Remove(returnedTask);
                }
            }

            // abwarten bis alle tasks fertig sind
            await Task.WhenAll(taskList);

            // ergebnisse anzeigen
            Message = "bin fertig mit Array füllen";
            Message += Environment.NewLine;
            foreach (var item in arrayToWorkOn[..10])
                Message += " " + item.ToString("D3");
            Message += Environment.NewLine;
            foreach (var item in arrayToWorkOn[^10..])
                Message += " " + item.ToString("D3");

            // Bereich Summe ziehen

            taskList.Clear();
            List<WorkPackage<ulong, byte>> sumPackages = new(WorkPackageNumber);

            sumPackages.Add(new WorkPackage<ulong, byte>(new ArraySegment<byte>(arrayToWorkOn, 0, segmentLengthRemainder)));
            for (int counter = 0; counter < WorkPackageNumber - 1; counter++)
                sumPackages.Add(new WorkPackage<ulong, byte>(new ArraySegment<byte>(arrayToWorkOn, segmentLengthRemainder + counter * segmentLength, segmentLength)));


            foreach (var item in sumPackages)
                ProgressBars.Add(item.ProgressBar);
            //    wpProgress.Children.Add(item.ProgressBar);

            int packageID = 0;
            LinkedList<Task<WorkPackage<ulong, byte>>> taskSumList = new();

            while (packageID < sumPackages.Count)
            {
                if (taskSumList.Count < Environment.ProcessorCount - 1)
                {
                    // tasks erstellen
                    var myPackage = sumPackages[packageID++]; // package aus der Queue holen
                    taskSumList.AddLast(Task.Run(() =>
                    {
                        ulong sum = 0;
                        int reportinterval = myPackage.Segment.Count / 100;
                        byte currentProgress = 0;
                        for (int counter = 0; counter < myPackage.Segment.Count; counter++)
                        {
                            sum += myPackage.Segment[counter];
                            if (counter % reportinterval == 0)
                            {
                                currentProgress++;
                                myPackage.Progress.Report(currentProgress);
                            }
                        }
                        myPackage.Result = sum;
                        return myPackage;
                    }));
                }
                else
                {
                    var finishedTask = await Task<WorkPackage<ulong, byte>>.WhenAny(taskSumList);
                    WorkPackage<ulong, byte> package = finishedTask.Result;
                    // warten bis einer fertig ist und dann recyclen
                    taskSumList.Remove(finishedTask);
                }
            }

            // abwarten bis alle tasks fertig sind
            await Task.WhenAll(taskSumList);

            ulong gesamtSumme = 0;
            foreach (var item in sumPackages) gesamtSumme += item.Result;
            Message += Environment.NewLine + $"Summe aller Arrayzellen: {gesamtSumme:#,0}";
            Message += Environment.NewLine + "Vergangene Zeit : " + (DateTime.Now - startTime).TotalSeconds.ToString("0.000");
        }
    }

    class WorkPackage
    {
        public WorkPackage(ArraySegment<byte> arraySegments)
        {
            Segment = arraySegments;
        }

        public ArraySegment<byte> Segment;
    }
    class WorkPackage<ReturnType, ArrayType>
    {
        public ReturnType Result;
        public ProgressBar ProgressBar;
        public IProgress<byte> Progress;
        public ArraySegment<ArrayType> Segment;
        public WorkPackage(ArraySegment<ArrayType> arraySegments)
        {
            Segment = arraySegments;
            ProgressBar = new ProgressBar();
            Progress = new Progress<byte>(report);
        }

        private void report(byte NewValue)
        {
            ProgressBar.Value = NewValue;
        }
    }

    class StartWorkQueueCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action MethodToStart;

        public StartWorkQueueCommand(Action MethodToStart)
        {
            this.MethodToStart = MethodToStart;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => MethodToStart?.Invoke();
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}
