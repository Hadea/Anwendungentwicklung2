using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeConditionalStyles.xaml
    /// </summary>
    public partial class pgeConditionalStyles : Page, INotifyPropertyChanged
    {
        private bool _appState;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ApplicationState
        {
            get { return _appState; }
            set
            {
                if (_appState != value)
                {
                    _appState = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApplicationState)));
                }
            }
        }


        public pgeConditionalStyles()
        {
            InitializeComponent();
            DataContext = this;
            ApplicationState = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationState = !ApplicationState;
        }
    }
}
