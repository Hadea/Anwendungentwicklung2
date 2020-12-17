using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeSound.xaml
    /// </summary>
    public partial class pgeSound : Page
    {
        MediaPlayer music;
        SoundPlayer sound;
        public pgeSound()
        {
            InitializeComponent();

            // MediaPlayer settings
            music = new MediaPlayer();
            string fileName = Environment.CommandLine;// liesst den absoluten pfad zu unserer Anwendung
            fileName = Directory.GetParent(fileName).FullName; // schneidet den Dateinamen von unserer Anwendung ab
            fileName = fileName + "/ShaolinDub-HarpDubz.mp3"; // setzt ans ende den Namen des Liedes
            music.Open(new Uri(fileName)); // lädt das lied, stellt es auf anfang, spielt es aber noch nicht ab
            music.MediaEnded += loop; // wenn das lied endet wird jede methode die in "MediaEndet" registriert ist aufgerufen

            // SoundPlayer settings
            sound = new SoundPlayer(); // erstellen des Objektes SoundPlayer
            sound.SoundLocation = "Klick.wav"; // laden der WAV datei aus dem gleichen Verzeichnis in dem auch das Programm liegt
        }

        /// <summary>
        /// Restarts the song.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void loop(object sender, EventArgs e)
        {
            music.Position = TimeSpan.Zero;
            music.Play();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            music.Play(); // startet die wiedergabe an der aktuellen position
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            music.Pause(); // pausiert die wiedergabe, sodass man an der stelle später weiterhören kann
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            music.Position = TimeSpan.Zero; // "spult" die datei wieder auf anfang zurück
            music.Pause(); // pausiert die datei direkt am anfang
        }

        private void btnClickSound_Click(object sender, RoutedEventArgs e)
        {
            sound.Play(); // startet den sound
        }
    }
}
