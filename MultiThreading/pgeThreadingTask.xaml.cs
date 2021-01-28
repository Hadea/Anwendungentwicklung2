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
    /// Interaction logic for pgeThreadingTask.xaml
    /// </summary>
    public partial class pgeThreadingTask : Page
    {
        Task threadA;
        Task threadB;
        Task threadC;
        Task threadD;

        volatile bool farbe = false;

        public pgeThreadingTask()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            threadA = new Task(threadAStuff);
            threadA.Start();
            rectB.Fill = Brushes.Red;

            for (int counter = 0; counter < 60; counter++)
            {
                await Task.Delay(100);
                rectA.Fill = farbe ? Brushes.Red : Brushes.Blue ;
            }
        }

        

        private void threadAStuff()
        {
            Thread.Sleep(5000);
            farbe = true;
        }
    }
}
