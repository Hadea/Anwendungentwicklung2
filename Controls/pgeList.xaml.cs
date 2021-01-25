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
        // der eigendliche datencontainer welcher nicht durch filter oder sortierung beeinflusst wird
        private readonly ObservableCollection<string> _items;

        // hilfsklasse welche zwischen ListView und den eigendlichen daten in _items zwischengeschaltet wird
        public ICollectionView ItemsView { get; init; }
        public pgeList()
        {
            InitializeComponent();
            DataContext = this;
            // befüllen der originalliste mit ein paar spieldaten
            _items = new ObservableCollection<string>();
            _items.Add("Alpha");
            _items.Add("Bravo");
            _items.Add("Charly");
            _items.Add("Delta");
            ItemsView = CollectionViewSource.GetDefaultView(_items);
        }

        private string _filter;

        // dieses Property ist mit der TextBox verbunden welche bei jeder änderung (PropertyChanged) den setter aufruft
        public string TextFilter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    // da sich der inhalt des filters ändert müssen wir diesen neu setzen.
                    // die Methode die in Filter eingehängt wird (delegate) wird für jedes Element in der
                    // original-liste _items ausgeführt und wenn true rauskommt wird dieses element an die
                    // anzeigende ListView weitergeleitet.
                    ItemsView.Filter = x => (x as string).Contains(_filter);
                }
            }
        }

    }
}
