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
    /// Interaction logic for pgeList.xaml
    /// </summary>
    public partial class pgeList : Page
    {
        ObservableCollection<string> Items;
        public ICollectionView ItemsView { get; init; }
        public pgeList()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<string>();
            Items.Add("Alpha");
            Items.Add("Bravo");
            Items.Add("Charly");
            Items.Add("Delta");
            ItemsView = CollectionViewSource.GetDefaultView(Items);
        }

        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    ItemsView.Filter = x => (x as string).Contains(_filter);
                }
            }
        }

    }
}
