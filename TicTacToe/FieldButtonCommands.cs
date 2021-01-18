using System.Windows.Input;

namespace TicTacToe
{
    public static class FieldButtonCommands
    {
        public static readonly RoutedUICommand Select = new RoutedUICommand(
            "FieldButton Selection",
            "Select",
            typeof(FieldButtonCommands));

    }
}
