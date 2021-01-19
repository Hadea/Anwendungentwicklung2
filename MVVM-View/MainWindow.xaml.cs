using MVVM_ViewModel;
using System.Windows;

namespace MVVM_View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).EntryList.Add(new UserViewModel { Name = "neu", Salary = 35000.0 });
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).EntryList[lvUser.SelectedIndex].Name = "geändert";
        }
    }
}
