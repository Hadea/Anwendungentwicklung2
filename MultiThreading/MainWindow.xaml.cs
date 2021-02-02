using System.Windows;
using System.Windows.Input;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICommand Command_NavigatePage { get; init; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Command_NavigatePage = new LoadPageCommand(this);
        }
    }
}
