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
            //long result = sumRandomArray(SumProgress, SumCancelSource.Token); // syncron, also "hängt" bis die aufgabe erledigt ist, erst danach ist das UI wieder nutzbar.
            long result = await Task<Int64>.Run(() => sumRandomArray(SumProgress, SumCancelSource.Token), SumCancelSource.Token);
            tbResult.Text = result.ToString();

            // normalerweise macht der GarbageCollector einen guten job und sollt nicht angetastet werden
            // in <1% der fälle haben wir als programmierer aber genug wissen wann unser system gerade eine
            // pause macht sodass wir das Aufräumen starten.
            // z.B. wenn der nutzer nach einer langen speicherintensiven operation ein ergebnis präsentiert
            // bekommt wird er es durchaus ein paar millisekunden anschauen, diese zeit können wir nutzen zum
            // aufräumen. (aber nur wenn es unbedingt nötig ist)

            // startet den GarbageCollektor
            //   -  auf allen generationen
            //   -  Forced zwingt ihn dazu es jetzt zu tun. Egal ob er vielleicht kurz vorher bereits automatisch lief
            //   -  blocking stellt ein ob die zeile syncron (warten bis fertig) oder asyncron (starten und im hintergrund arbeiten) laufen soll.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, false);
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
                if (position % (randomArray.Length / 700) == 0)
                {
                    sumProgress.Report(++progressPerMille);
                    if (token.IsCancellationRequested) return 0;
                }
            }

            long sum = 0;
            for (int position = 0; position < randomArray.Length; position++)
            {
                sum += randomArray[position];
                if (position % (randomArray.Length / 300) == 0) // nur 300 mal pro gesamtdurchlauf aktualisieren
                {
                    // report gibt dem ui-thread bescheid das es etwas zu aktualisieren gibt
                    // wird dies zu häufig gemacht können wir den UI-Thread überlasten
                    // hilfreich ist dabei immer in Frames pro Sekunde (FPS) umzurechnen
                    // 1000x Report während der Thread 4 sekunden läuft würde 250 FPS-Monitor
                    // benötigen um komplett dargestellt zu werden.
                    // Guter geschätzter wert währe 30 Reports pro sekunde auf der schlechtesten
                    // unterstützten Hardware (min: 1GHz, 2GB Ram)

                    sumProgress.Report(++progressPerMille);
                    
                    // da auch die abfrage nach dem canceltoken etwas zeit kostet wird das zeitgleich mit dem fortschritt erledigt
                    if (token.IsCancellationRequested)
                        return sum;
                }
            }
            return sum;
        }
    }
}
