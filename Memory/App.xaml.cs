using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Memory
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow w;

        private void AppStart(object sender, StartupEventArgs e)
        {
            w = new MainWindow(new HighScoreStorageSqlite());
            w.Show();
        }
    }
}
