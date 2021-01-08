using System.Windows;

namespace Memory
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow w;

        private void appStart(object sender, StartupEventArgs e)
        {
            w = new MainWindow(new HighScoreStorageSqlite()); // hier ist die einzige stelle wo das Objekt zum passenden Interface eingefügt wird
            w.Show(); // das Window muss explizit angezeigt werden, im gegensatz zum XAML-Weg mit StartupURI
        }
    }
}
