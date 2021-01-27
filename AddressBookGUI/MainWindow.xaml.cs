using System.Windows;
using System.Windows.Controls;

namespace AddressBookGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // dieser code wird nur ausgeführt wenn wir im Debug-Modus von Visual Studio sind.
            // im Realeasemode wird die Zeile nicht mitcompiliert.
            mnuMainMenu.Items.Add(new MenuItem() { Header = "Cheats" });
#endif

        }

    }
}
