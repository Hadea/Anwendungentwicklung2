using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Controls
{
    /// <summary>
    /// Interaction logic for pgeTreeView.xaml
    /// </summary>
    public partial class pgeTreeView : Page
    {
        public ObservableCollection<TreeContainer> Tree { get; init; } = new();

        public pgeTreeView()
        {
            InitializeComponent();
            DataContext = this;

            Tree.Add(new TreeContainer() { Name = "Container 1" });
            Tree[0].SubNodes.Add(new TreeContainer() { Name = "Alpha" });
            Tree[0].SubNodes.Add(new TreeContainer() { Name = "Bravo" });
            Tree[0].SubNodes.Add(new TreeContainer() { Name = "Charly" });

            Tree[0].SubNodes[1].SubNodes.Add(new TreeContainer() { Name = "Gamma" });
            Tree[0].SubNodes[1].SubNodes.Add(new TreeContainer() { Name = "Hotel" });

            Tree.Add(new TreeContainer() { Name = "Container 2" });
            Tree[1].SubNodes.Add(new TreeContainer() { Name = "Delta" });
            Tree[1].SubNodes.Add(new TreeContainer() { Name = "Echo" });

            Tree.Add(new TreeContainer() { Name = "Container 3" });
            Tree[2].SubNodes.Add(new TreeContainer() { Name = "Foxtrott" });

            Tree.Add(new TreeContainer() { Name = "Container 4" });
        }
    }

    public class TreeContainer
    {
        public ObservableCollection<TreeContainer> SubNodes { get; init; } = new();
        public string Name { get; set; }
    }
}
