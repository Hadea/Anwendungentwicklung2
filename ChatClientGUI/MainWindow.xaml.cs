using System.Windows;

namespace ChatClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChatViewModel vm = new();
            vm.ScrollDownMethod = svMessages.ScrollToBottom;
            vm.UIDispatcher =  Dispatcher;
            DataContext = vm;
        }
    }
}
