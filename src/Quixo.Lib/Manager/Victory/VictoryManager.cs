using Quixo.Lib.Model;
using Quixo.Lib.Enum;

namespace Quixo.Lib.Manager.Victory
{
    /// <summary>
    /// Checks victory conditions on a Quixo board.
    /// </summary>
    public class VictoryManager : IVictoryManager
    {
        /// <summary>
        /// Gets the board used for victory checks.
        /// </summary>
        private Board Board { get; }

        /// <summary>
        /// Event triggered when a victory is detected.
        /// </summary>
        public event Action<Symbol>? GameFinished;

        /// <summary>
        /// Initializes a new instance of the VictoryManager class.
        /// </summary>
        /// <param name="board">Board to analyze.</param>
        public VictoryManager(Board board)
        {
            Board = board;
        }

        /// <summary>
        /// Checks whether a row contains five identical non-empty symbols.
        /// </summary>
        /// <param name="x">Index of the row to check.</param>
        /// <returns>The winning symbol, or Symbol.NONE.</returns>
        public Symbol CheckRow(int x)
        {
            Symbol firstSymbol = Board.Cubes[x, 0].Symbol;
            if (firstSymbol == Symbol.NONE) return Symbol.NONE;

            for (int i = 1; i < 5; i++)
            {
                Symbol symbol = Board.Cubes[x, i].Symbol;
                if (symbol != firstSymbol) return Symbol.NONE;
            }

            GameFinished?.Invoke(firstSymbol);

            return firstSymbol;
        }

        /// <summary>
        /// Checks whether a column contains five identical non-empty symbols.
        /// </summary>
        /// <param name="y">Index of the column to check.</param>
        /// <returns>The winning symbol, or Symbol.NONE.</returns>
        public Symbol CheckColumn(int y)
        {
            Symbol firstSymbol = Board.Cubes[0, y].Symbol;
            if (firstSymbol == Symbol.NONE) return Symbol.NONE;

            for (int i = 1; i < 5; i++)
            {
                Symbol symbol = Board.Cubes[i, y].Symbol;
                if (symbol != firstSymbol) return Symbol.NONE;
            }

            GameFinished?.Invoke(firstSymbol);
            return firstSymbol;
        }

        /// <summary>
        /// Checks the main diagonal (top-left to bottom-right).
        /// </summary>
        /// <returns>The winning symbol, or Symbol.NONE.</returns>
        private Symbol CheckRightDiag()
        {
            Symbol firstSymbol = Board.Cubes[0, 0].Symbol;
            if (firstSymbol == Symbol.NONE) return Symbol.NONE;

            for (int i = 1; i < 5; i++)
            {
                Symbol symbol = Board.Cubes[i, i].Symbol;
                if (symbol != firstSymbol) return Symbol.NONE;
            }

            GameFinished?.Invoke(firstSymbol);
            return firstSymbol;
        }

        /// <summary>
        /// Checks the secondary diagonal (top-right to bottom-left).
        /// </summary>
        /// <returns>The winning symbol, or Symbol.NONE.</returns>
        private Symbol CheckLeftDiag()
        {
            Symbol firstSymbol = Board.Cubes[0, 4].Symbol;
            if (firstSymbol == Symbol.NONE) return Symbol.NONE;

            for (int x = 1, y = 3; x < 5; x++, y--)
            {
                Symbol symbol = Board.Cubes[x, y].Symbol;
                if (symbol != firstSymbol) return Symbol.NONE;
            }

            GameFinished?.Invoke(firstSymbol);
            return firstSymbol;
        }

        /// <summary>
        /// Checks both diagonals of the board.
        /// </summary>
        /// <returns>The winning symbol, or Symbol.NONE.</returns>
        public Symbol CheckDiag()
        {
            Symbol rightDiag = CheckRightDiag();
            if (rightDiag != Symbol.NONE) return rightDiag;

            Symbol leftDiag = CheckLeftDiag();
            if (leftDiag != Symbol.NONE) return leftDiag;

            return Symbol.NONE;
        }
    }
}