using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeLogic;

namespace TicTacToeTest
{
    [TestClass]
    public class TicTacToe
    {
        [TestMethod]
        public void GameCreation()
        {
            GameLogic l = new GameLogic();
            Assert.IsNotNull(l.GetCurrentPlayer());
        }

        [TestMethod]
        public void GameFieldReturn()
        {
            GameLogic l = new GameLogic();
            l.Reset();
            Assert.IsNotNull(l.GetBoard());
        }

        [TestMethod]
        public void GameFieldCount()
        {
            GameLogic l = new GameLogic();
            l.Reset();
            Assert.IsTrue(l.GetBoard().Length == 9);
        }

        [TestMethod]
        public void RandomPlayerStart()
        {
            GameLogic l = new GameLogic();
            bool firstPlayer = l.GetCurrentPlayer();
            bool result = false;
            for (int count = 0; count < 20; count++)
            {
                l.Reset();
                if (l.GetCurrentPlayer() != firstPlayer)
                {
                    result = true;
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TurnFirstMoveMustBeValid()
        {
            GameLogic l = new GameLogic();
            for (byte x = 0; x < 3; x++)
            {
                for (byte y = 0; y < 3; y++)
                {
                    l.Reset();
                    Assert.IsTrue(l.Turn(new PointB(x, y)) == TurnResult.Valid);
                }
            }
        }

        [TestMethod]
        public void TurnInvalid()
        {
            GameLogic l = new GameLogic();
            l.Reset();
            l.Turn(new PointB(0, 0));
            Assert.IsTrue(l.Turn(new PointB(0, 0)) == TurnResult.Invalid);
        }

        [TestMethod]
        public void TurnTie()
        {
            // x o x
            // x o x
            // o x o
            GameLogic l = new GameLogic();
            l.Reset();
            Assert.IsTrue(l.Turn(new PointB(0, 0)) == TurnResult.Valid); // X
            Assert.IsTrue(l.Turn(new PointB(1, 0)) == TurnResult.Valid); // O
            Assert.IsTrue(l.Turn(new PointB(2, 0)) == TurnResult.Valid); // X

            Assert.IsTrue(l.Turn(new PointB(1, 1)) == TurnResult.Valid); // O
            Assert.IsTrue(l.Turn(new PointB(0, 1)) == TurnResult.Valid); // X
            Assert.IsTrue(l.Turn(new PointB(0, 2)) == TurnResult.Valid); // O

            Assert.IsTrue(l.Turn(new PointB(2, 1)) == TurnResult.Valid); // X
            Assert.IsTrue(l.Turn(new PointB(2, 2)) == TurnResult.Valid); // O
            Assert.IsTrue(l.Turn(new PointB(1, 2)) == TurnResult.Tie); // X
        }
        [TestMethod]
        public void TurnWinHorizontal()
        {
            // X X X
            // O O
            //

            GameLogic l = new GameLogic();
            l.Reset();
            l.Turn(new PointB(0, 0)); // X
            l.Turn(new PointB(0, 1)); // O
            l.Turn(new PointB(1, 0)); // X
            l.Turn(new PointB(1, 1)); // O
            Assert.IsTrue(l.Turn(new PointB(2, 0)) == TurnResult.Win);
        }

        [TestMethod]
        public void TurnWinVertical()
        {
            // X O
            // X O
            // X
            GameLogic l = new GameLogic();
            l.Reset();
            l.Turn(new PointB(0, 0)); // X
            l.Turn(new PointB(1, 0)); // O
            l.Turn(new PointB(0, 1)); // X
            l.Turn(new PointB(1, 1)); // O
            Assert.IsTrue(l.Turn(new PointB(0, 2)) == TurnResult.Win);
        }

        [TestMethod]
        public void TurnWinDiagonal()
        {
            // X O
            // O X
            //     X
            GameLogic l = new GameLogic();
            l.Reset();
            l.Turn(new PointB(0, 0)); // X
            l.Turn(new PointB(0, 1)); // O
            l.Turn(new PointB(1, 1)); // X
            l.Turn(new PointB(1, 0)); // O
            Assert.IsTrue(l.Turn(new PointB(2, 2)) == TurnResult.Win);
        }
    }
}
