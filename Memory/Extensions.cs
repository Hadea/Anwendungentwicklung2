using System.Windows.Controls;

namespace Memory
{
    public static class Extensions
    {
        /// <summary>
        /// Clears RowDefinitions, ColumnDefinitions and Childrend of the <typeparamref name="grid"/>.
        /// </summary>
        public static void Clear(this Grid grid) // erweitert Grid um die Methode Clear sodass jedes Grid im zusammenhang mit dem Namespace "Memory" mehr kann.
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
        }
    }
}
