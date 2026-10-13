using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Placement
{
    /// <summary>
    /// Defines operations for validating and executing board movements.
    /// </summary>
    public interface IPlacementManager
    {
        /// <summary>
        /// Event triggered when the board state changes.
        /// </summary>
        public event Action<Board>? BoardChanged;

        /// <summary>
        /// Event triggered when the active player changes.
        /// </summary>
        public event Action<Player>? ActivePlayerChanged;

        /// <summary>
        /// Checks whether a movement is valid.
        /// </summary>
        /// <param name="m">Movement to validate.</param>
        /// <returns>true if the movement is allowed; otherwise, false.</returns>
        public bool CheckMovement(Movement m);

        /// <summary>
        /// Executes a valid movement on the board.
        /// </summary>
        /// <param name="m">Movement to apply.</param>
        public void MoveCubes(Movement m);

        /// <summary>
        /// Switches to the next player.
        /// </summary>
        public void SwitchPlayer();

        /// <summary>
        /// Notifies observers that the board content has been updated externally.
        /// </summary>
        public void NotifyBoardChanged();
    }
}