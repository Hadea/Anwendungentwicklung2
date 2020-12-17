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
        MediaPlayer[] soundPlayers;
        List<string[]> soundSets;

        public MainWindow()
        {
            InitializeComponent();
            soundPlayers = new MediaPlayer[9];
            for (int counter = 0; counter < soundPlayers.Length; counter++)
            {
                soundPlayers[counter] = new MediaPlayer();
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
        }

        private void loadSoundSet(int SetId)
        {
            for (int counter = 0; counter < soundPlayers.Length; counter++)
            {
                soundPlayers[counter].Open(new Uri(Directory.GetParent(Environment.CommandLine).FullName + soundSets[SetId][counter]));
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {

            switch ((sender as Button).Name)
            {
                case "btnPlay0":
                    soundPlayers[0].Position = TimeSpan.Zero;
                    soundPlayers[0].Play();
                    break;
                case "btnPlay1":
                    soundPlayers[1].Position = TimeSpan.Zero;
                    soundPlayers[1].Play();
                    break;
                case "btnPlay2":
                    soundPlayers[2].Position = TimeSpan.Zero;
                    soundPlayers[2].Play();
                    break;
                case "btnPlay3":
                    soundPlayers[3].Position = TimeSpan.Zero;
                    soundPlayers[3].Play();
                    break;
                case "btnPlay4":
                    soundPlayers[4].Position = TimeSpan.Zero;
                    soundPlayers[4].Play();
                    break;
                case "btnPlay5":
                    soundPlayers[5].Position = TimeSpan.Zero;
                    soundPlayers[5].Play();
                    break;
                case "btnPlay6":
                    soundPlayers[6].Position = TimeSpan.Zero;
                    soundPlayers[6].Play();
                    break;
                case "btnPlay7":
                    soundPlayers[7].Position = TimeSpan.Zero;
                    soundPlayers[7].Play();
                    break;
                case "btnPlay8":
                    soundPlayers[8].Position = TimeSpan.Zero;
                    soundPlayers[8].Play();
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
            foreach (var item in soundPlayers)
            {
                item.Pause();
                item.Position = TimeSpan.Zero;
            }
        }
    }
}
