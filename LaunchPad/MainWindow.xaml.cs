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

            firstSet[0] = "/SoundSets/SoundSet1/String-2080string.wav";
            firstSet[1] = "/SoundSets/SoundSet1/String-CamenbertString.wav";
            firstSet[2] = "/SoundSets/SoundSet1/String-CrunchyWarmth.wav";
            firstSet[3] = "/SoundSets/SoundSet1/String-MoonliteBreeze.wav";
            firstSet[4] = "/SoundSets/SoundSet1/Strings-Silksparkle.wav";
            firstSet[5] = "/SoundSets/SoundSet1/Strings-SilksparkleDIfferent.wav";
            firstSet[6] = "/SoundSets/SoundSet1/Strings-SilksparkleDIfferentLow.wav";
            firstSet[7] = "/SoundSets/SoundSet1/String-SweetBuckets.wav";
            firstSet[8] = "/SoundSets/SoundSet1/String-TurtleBeach.wav";

            soundSets.Add(firstSet);
            loadSoundSet(0);
        }

        private void loadSoundSet(int SetId)
        {
            for (int counter = 0; counter < soundPlayers.Length; counter++)
            {
                soundPlayers[counter].Open( new Uri( Directory.GetParent(Environment.CommandLine).FullName+ soundSets[SetId][counter]));
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            
            switch ((sender as Button).Name)
            {
                case "btnPlay0":
                    soundPlayers[0].Play();
                    break;
                case "btnPlay1":
                    soundPlayers[1].Play();
                    break;
                case "btnPlay2":
                    soundPlayers[2].Play();
                    break;
                case "btnPlay3":
                    soundPlayers[3].Play();
                    break;
                case "btnPlay4":
                    soundPlayers[4].Play();
                    break;
                case "btnPlay5":
                    soundPlayers[5].Play();
                    break;
                case "btnPlay6":
                    soundPlayers[6].Play();
                    break;
                case "btnPlay7":
                    soundPlayers[7].Play();
                    break;
                case "btnPlay8":
                    soundPlayers[8].Play();
                    break;
            }
        }
    }
}
