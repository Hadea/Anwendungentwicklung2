using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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


        public pgeThreadingTask()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            rectA.Fill = Brushes.Blue;
            rectB.Fill = Brushes.Blue;
            rectC.Fill = Brushes.Blue;
            rectD.Fill = Brushes.Blue;
            threadA = new Task(()=>threadAStuff(rectA));
            threadB = new Task(()=>threadAStuff(rectB));
            threadC = new Task(()=>threadAStuff(rectC));
            threadD = new Task(()=>threadAStuff(rectD));
            threadA.Start();
            threadB.Start();
            threadC.Start();
            threadD.Start();
        }

        private void threadAStuff(Rectangle Rect)
        {
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(3500,6000));
            // direkter zugriff auf Objekte aus dem UI-Thread funktionieren nicht. 
            // nicht einmal wenn die gesetzten variablen explizit innerhalb des UI-Thread gelesen werden.
            // Rect.Fill = Brushes.Red; 

            // Mit Dispatcher.Invoke teilen wir dem UI-Thread mit das er eine methode ausführen soll.
            Dispatcher.Invoke(() => Rect.Fill = Brushes.Red);
        }

        private void btnGetColor_Click(object sender, RoutedEventArgs e)
        {
            rectOut.Fill = rectA.Fill;
        }
    }
}
