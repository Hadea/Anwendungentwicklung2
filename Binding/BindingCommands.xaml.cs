using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Binding
{
    /// <summary>
    /// Interaction logic for BindingCommands.xaml
    /// </summary>
    public partial class BindingCommands : Page
    {
        public BindingCommands()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Es wurde gespeichert", "Hat geklappt", MessageBoxButton.OK, MessageBoxImage.Information);
            editBoxDirty = false;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = editBoxDirty;
        }

        private string editBoxVal;
        private bool editBoxDirty;
        public string EditFieldValue
        {
            get { return editBoxVal; }
            set
            {
                if (editBoxVal != value)
                {
                    editBoxDirty = true;
                    editBoxVal = value;
                }
            }
        }

        private bool IsAlert = false;
        private void RedAlert_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IsAlert = !IsAlert;
            Background = IsAlert ? Brushes.Red : Brushes.White;
        }

        Window subWindow;
        private void OpenWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            subWindow = new MenuCommands();
            subWindow.Show();
        }
    }
}
