using System.Windows;
using TaschenrechnerLogic;

namespace TaschenrechnerView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TaschenrechnerViewModel(); // startet die logik und bindet sie als standardelement für bindungen ein
        }
    }
}
