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
            using (StreamReader reader = new StreamReader("SoundSets/SoundSets.txt"))
            {
                int soundCounter = 0;
                string[] soundSet = null;
                string fileName;
                while ((fileName = reader.ReadLine()) != null)
                {
                    if (soundCounter == 0)
                    {
                        soundSet = new string[9];
                        soundSets.Add(soundSet);
                    }
                    soundSet[soundCounter] = fileName;
                    soundCounter++;

                    if (soundCounter >= soundSet.Length)
                    {
                        soundCounter = 0;
                    }
                }
            }

            loadSoundSet(0);

            for (int counter = 0; counter < soundSets.Count; counter++)
            {
                var temp = new RadioButton
                {
                    Content = "Set " + counter,
                    GroupName = "soundSetGroup",
                    Name = "Set" + counter
                };
                temp.Click += rbSoundSet_Click;
                temp.IsChecked = counter == 0;
                radioContainer.Children.Add(temp);
            }

            Grid contentGrid = Content as Grid;
            int buttonCounter = 0;
            List<Button> buttons = new List<Button>(9);
            for (int rows = 2; rows < 5; rows++)
            {
                for (int cols = 0; cols < 3; cols++)
                {
                    var temp = new Button
                    {
                        Name = "btnPlay" + buttonCounter++,
                        Style = FindResource("PadButtons") as Style
                    };
                    Grid.SetRow(temp,rows);
                    Grid.SetColumn(temp,cols);
                    contentGrid.Children.Add(temp);
                    buttons.Add(temp);
                }
            }

            mediaPlayers[0].MediaEnded += (o, e) => buttons[0].Background = Brushes.LightGray;
            mediaPlayers[1].MediaEnded += (o, e) => buttons[1].Background = Brushes.LightGray;
            mediaPlayers[2].MediaEnded += (o, e) => buttons[2].Background = Brushes.LightGray;
            mediaPlayers[3].MediaEnded += (o, e) => buttons[3].Background = Brushes.LightGray;
            mediaPlayers[4].MediaEnded += (o, e) => buttons[4].Background = Brushes.LightGray;
            mediaPlayers[5].MediaEnded += (o, e) => buttons[5].Background = Brushes.LightGray;
            mediaPlayers[6].MediaEnded += (o, e) => buttons[6].Background = Brushes.LightGray;
            mediaPlayers[7].MediaEnded += (o, e) => buttons[7].Background = Brushes.LightGray;
            mediaPlayers[8].MediaEnded += (o, e) => buttons[8].Background = Brushes.LightGray;
        }

        private void loadSoundSet(int SetId)
        {
            for (int counter = 0; counter < mediaPlayers.Length; counter++)
            {
                mediaPlayers[counter].Open(new Uri(Directory.GetParent(Environment.CommandLine).FullName + soundSets[SetId][counter]));
            }

            for (int counter = 0; counter < (this.Content as Grid).Children.Count; counter++)
            {
                if (((this.Content as Grid).Children[counter] as Button) != null && ((this.Content as Grid).Children[counter] as Button).Name.Contains("btnPlay"))
                {
                    ((this.Content as Grid).Children[counter] as Button).Background = Brushes.LightGray;
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

        private void rbSoundSet_Click(object sender, RoutedEventArgs e)
        {
            loadSoundSet(int.Parse((sender as RadioButton).Name[3..]));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in mediaPlayers)
            {
                item.Pause();
                item.Position = TimeSpan.Zero;
            }

            for (int counter = 0; counter < (this.Content as Grid).Children.Count; counter++)
            {
                if (((this.Content as Grid).Children[counter] as Button) != null && ((this.Content as Grid).Children[counter] as Button).Name.Contains("btnPlay"))
                {
                    ((this.Content as Grid).Children[counter] as Button).Background = Brushes.LightGray;
                }
            }
        }
    }
}
