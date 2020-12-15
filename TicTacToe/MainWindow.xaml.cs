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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameLogic logic;
        Button[,] buttonArray;
        public MainWindow()
        {
            InitializeComponent();
            logic = new();
            buttonArray = new Button[3, 3];
            buttonArray[0, 0] = btnField11;
            buttonArray[0, 1] = btnField21;
            buttonArray[0, 2] = btnField31;
            buttonArray[1, 0] = btnField12;
            buttonArray[1, 1] = btnField22;
            buttonArray[1, 2] = btnField32;
            buttonArray[2, 0] = btnField13;
            buttonArray[2, 1] = btnField23;
            buttonArray[2, 2] = btnField33;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            logic.Reset();
            foreach (var item in buttonArray)
            {
                item.Content = "";
                item.IsEnabled = true;
            }
        }

        private void redrawField()
        {
            var board = logic.GetBoard();
            buttonArray[0, 0].Content = board[0, 0] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[1, 0].Content = board[1, 0] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[2, 0].Content = board[2, 0] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[0, 1].Content = board[0, 1] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[1, 1].Content = board[1, 1] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[2, 1].Content = board[2, 1] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[0, 2].Content = board[0, 2] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[1, 2].Content = board[1, 2] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
            buttonArray[2, 2].Content = board[2, 2] switch { FieldState.X => "X", FieldState.O => "O", _ => "" };
        }

        private bool evaluateTurn(TurnResult Result)
        {
            switch (Result)
            {
                case TurnResult.Valid:
                    redrawField();
                    return false;
                case TurnResult.Invalid:
                    lblMessage.Content = "Das war nix";
                    break;
                case TurnResult.Win:
                    redrawField();
                    lblMessage.Content = $"Sieg! Spieler {(logic.GetCurrentPlayer() ? "X" : "O")} hat gewonnen";
                    foreach (var item in buttonArray)
                    {
                        item.IsEnabled = false;
                    }
                    return false;
                case TurnResult.Tie:
                    redrawField();
                    lblMessage.Content = "Unentschieden!";
                    foreach (var item in buttonArray)
                    {
                        item.IsEnabled = false;
                    }
                    return false;
            }
            return true;
        }

        private void btnField11_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 0)));
        private void btnField21_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 0)));
        private void btnField31_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 0)));
        private void btnField12_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 1)));
        private void btnField22_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 1)));
        private void btnField32_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 1)));
        private void btnField13_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(0, 2)));
        private void btnField23_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(1, 2)));
        private void btnField33_Click(object sender, RoutedEventArgs e) => (sender as Button).IsEnabled = evaluateTurn(logic.Turn(new PointB(2, 2)));

    }
}
