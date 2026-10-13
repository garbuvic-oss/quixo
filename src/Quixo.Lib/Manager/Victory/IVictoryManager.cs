using Quixo.Lib.Enum;

namespace Quixo.Lib.Manager.Victory
{
    /// <summary>
    /// Defines victory-check operations on the board.
    /// </summary>
    public interface IVictoryManager
    {
        /// <summary>
        /// Event triggered when a winning symbol is detected.
        /// </summary>
        public event Action<Symbol>? GameFinished;

        /// <summary>
        /// Checks whether a row contains a winning combination.
        /// </summary>
        /// <param name="x">Index of the row to check.</param>
        /// <returns>The winning symbol, or Symbol.NONE if there is no victory.</returns>
        public Symbol CheckRow(int x);

        /// <summary>
        /// Checks whether a column contains a winning combination.
        /// </summary>
        /// <param name="y">Index of the column to check.</param>
        /// <returns>The winning symbol, or Symbol.NONE if there is no victory.</returns>
        public Symbol CheckColumn(int y);

        /// <summary>
        /// Checks both board diagonals.
        /// </summary>
        /// <returns>The winning symbol, or Symbol.NONE if there is no victory.</returns>
        public Symbol CheckDiag();
    }
}