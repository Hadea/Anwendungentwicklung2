using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for pgeListFilter.xaml
    /// </summary>
    public partial class pgeListFilter : Page, INotifyPropertyChanged
    {
        public ObservableCollection<UserViewModel> Items { get; init; }
        public pgeListFilter()
        {
            InitializeComponent();
            DataContext = this;
            Items = new();
            Items.Add(new UserViewModel {FirstName="Blubber", LastName = "Alpha" });
            Items.Add(new UserViewModel {FirstName="Blubber", LastName = "Bravo" });
            Items.Add(new UserViewModel {FirstName="Blubber", LastName = "Charly" });
            Items.Add(new UserViewModel {FirstName="Blubber", LastName = "Delta" });
        }

        private void cvsItems_Filter(object sender, FilterEventArgs e)
        {
            UserViewModel converted = e.Item as UserViewModel;
            e.Accepted = ItemFilter == null || (converted != null && converted.LastName != null && converted.LastName.Contains(ItemFilter));
            //e.Accepted = true;
        }

        private string _itemFilter;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ItemFilter
        {
            get { return _itemFilter; }
            set
            {
                _itemFilter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemFilter)));
                (FindResource("cvsItems") as CollectionViewSource).View.Refresh();
            }
        }

    }
}
