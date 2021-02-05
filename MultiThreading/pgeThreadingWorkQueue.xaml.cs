﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        }

        public int WorkPackageNumber { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

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


        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Message = "Starte";
            // vorbereitungen
            byte[] arrayToWorkOn = new byte[500_000_000];

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
                    taskList.AddLast(Task.Run(() => {
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
            Message = "bin fertig";
            Message += Environment.NewLine;
            foreach (var item in arrayToWorkOn[..10])
                Message += " " +item.ToString("D3");
            Message += Environment.NewLine;
            foreach (var item in arrayToWorkOn[^10..])
                Message += " "+ item.ToString("D3");
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
}
