using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
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

namespace LaunchPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MediaPlayer[] mediaPlayers;
        readonly List<string[]> soundSets;

        public MainWindow()
        {
            InitializeComponent();
            mediaPlayers = new MediaPlayer[9];
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter] = new MediaPlayer();
            }

            soundSets = new List<string[]>();
            string[] firstSet = new string[9];

            firstSet[0] = "/SoundSets/SoundSet0/mhak kick 106 F.wav";
            firstSet[1] = "/SoundSets/SoundSet0/mhak kick 14 F.wav";
            firstSet[2] = "/SoundSets/SoundSet0/mhak kick 140 F.wav";
            firstSet[3] = "/SoundSets/SoundSet0/mhak kick 35 F.wav";
            firstSet[4] = "/SoundSets/SoundSet0/mhak kick 46 F.wav";
            firstSet[5] = "/SoundSets/SoundSet0/mhak kick 63 F.wav";
            firstSet[6] = "/SoundSets/SoundSet0/mhak kick 81 F.wav";
            firstSet[7] = "/SoundSets/SoundSet0/mhak kick 89 F.wav";
            firstSet[8] = "/SoundSets/SoundSet0/mhak kick 02 F.wav";

            soundSets.Add(firstSet);

            string[] secondSet = new string[9];
            secondSet[0] = "/SoundSets/SoundSet1/String-2080string.wav";
            secondSet[1] = "/SoundSets/SoundSet1/String-CamenbertString.wav";
            secondSet[2] = "/SoundSets/SoundSet1/String-CrunchyWarmth.wav";
            secondSet[3] = "/SoundSets/SoundSet1/String-MoonliteBreeze.wav";
            secondSet[4] = "/SoundSets/SoundSet1/Strings-Silksparkle.wav";
            secondSet[5] = "/SoundSets/SoundSet1/Strings-SilksparkleDIfferent.wav";
            secondSet[6] = "/SoundSets/SoundSet1/Strings-SilksparkleDIfferentLow.wav";
            secondSet[7] = "/SoundSets/SoundSet1/String-SweetBuckets.wav";
            secondSet[8] = "/SoundSets/SoundSet1/String-TurtleBeach.wav";
            soundSets.Add(secondSet);
            loadSoundSet(0);

            mediaPlayers[0].MediaEnded += (o, e) => btnPlay0.Background = Brushes.DarkRed;
            mediaPlayers[1].MediaEnded += (o, e) => btnPlay1.Background = Brushes.DarkRed;
            mediaPlayers[2].MediaEnded += (o, e) => btnPlay2.Background = Brushes.DarkRed;
            mediaPlayers[3].MediaEnded += (o, e) => btnPlay3.Background = Brushes.DarkRed;
            mediaPlayers[4].MediaEnded += (o, e) => btnPlay4.Background = Brushes.DarkRed;
            mediaPlayers[5].MediaEnded += (o, e) => btnPlay5.Background = Brushes.DarkRed;
            mediaPlayers[6].MediaEnded += (o, e) => btnPlay6.Background = Brushes.DarkRed;
            mediaPlayers[7].MediaEnded += (o, e) => btnPlay7.Background = Brushes.DarkRed;
            mediaPlayers[8].MediaEnded += (o, e) => btnPlay8.Background = Brushes.DarkRed;
        }

        private void loadSoundSet(int SetId)
        {
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter].Open(new Uri(Directory.GetParent(Environment.CommandLine).FullName + soundSets[SetId][counter]));
            }

            for (int counter = 0; counter < (this.Content as Grid).Children.Count; counter++)
            {
                if (((this.Content as Grid).Children[counter] as Button) != null  && ((this.Content as Grid).Children[counter] as Button).Name.Contains("btnPlay"))
                {
                    ((this.Content as Grid).Children[counter] as Button).Background = Brushes.Gray;
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            (sender as Button).Background = Brushes.DarkGreen;
            switch ((sender as Button).Name)
            {
                case "btnPlay0":
                    mediaPlayers[0].Position = TimeSpan.Zero;
                    mediaPlayers[0].Play();
                    break;
                case "btnPlay1":
                    mediaPlayers[1].Position = TimeSpan.Zero;
                    mediaPlayers[1].Play();
                    break;
                case "btnPlay2":
                    mediaPlayers[2].Position = TimeSpan.Zero;
                    mediaPlayers[2].Play();
                    break;
                case "btnPlay3":
                    mediaPlayers[3].Position = TimeSpan.Zero;
                    mediaPlayers[3].Play();
                    break;
                case "btnPlay4":
                    mediaPlayers[4].Position = TimeSpan.Zero;
                    mediaPlayers[4].Play();
                    break;
                case "btnPlay5":
                    mediaPlayers[5].Position = TimeSpan.Zero;
                    mediaPlayers[5].Play();
                    break;
                case "btnPlay6":
                    mediaPlayers[6].Position = TimeSpan.Zero;
                    mediaPlayers[6].Play();
                    break;
                case "btnPlay7":
                    mediaPlayers[7].Position = TimeSpan.Zero;
                    mediaPlayers[7].Play();
                    break;
                case "btnPlay8":
                    mediaPlayers[8].Position = TimeSpan.Zero;
                    mediaPlayers[8].Play();
                    break;
            }
        }

        private void rbSoundSet0_Click(object sender, RoutedEventArgs e)
        {
            loadSoundSet(0);
        }

        private void rbSoundSet1_Click(object sender, RoutedEventArgs e)
        {
            loadSoundSet(1);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in mediaPlayers)
            {
                item.Pause();
                item.Position = TimeSpan.Zero;
            }
        }
    }
}
