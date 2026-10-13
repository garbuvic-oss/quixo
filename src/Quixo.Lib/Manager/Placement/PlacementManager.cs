using Quixo.Lib.Model;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Grid;

namespace Quixo.Lib.Manager.Placement
{
    /// <summary>
    /// Implements the logic for validating and applying board movements.
    /// </summary>
    public class PlacementManager(Game game, GridManager gManager) : IPlacementManager
    {
        private readonly Game game = game;
        private readonly Board? board = gManager.Board;

        /// <summary>
        /// Event triggered when the board is updated.
        /// </summary>
        public event Action<Board>? BoardChanged;

        /// <summary>
        /// Event triggered when the active player changes.
        /// </summary>
        public event Action<Player>? ActivePlayerChanged;

        /// <summary>
        /// Checks that a movement is allowed according to Quixo rules.
        /// </summary>
        /// <param name="m">Movement to validate.</param>
        /// <returns>true if the movement is valid; otherwise, false.</returns>
        public bool CheckMovement(Movement m)
        {
            if (board is null)
            {
                return false;
            }

            if (!IsSelectableEdge(m))
            {
                return false;
            }

            if (IsBlockedByBoardBoundary(m))
            {
                return false;
            }

            return !HasFrozenCubeOnPath(m);
        }

        private bool IsSelectableEdge(Movement m)
        {
            Symbol symbol = board!.GetSymbol(m.X, m.Y);
            Symbol currentSymbol = game.CurrentPlayer.Symbol;

            return GridManager.IsEdge(m.X, m.Y)
                && (symbol == Symbol.NONE || symbol == currentSymbol);
        }

        private static bool IsBlockedByBoardBoundary(Movement m)
        {
            return (m.X == 0 && m.Direction == Direction.LEFT)
                || (m.X == 4 && m.Direction == Direction.RIGHT)
                || (m.Y == 0 && m.Direction == Direction.UP)
                || (m.Y == 4 && m.Direction == Direction.DOWN);
        }

        private bool HasFrozenCubeOnPath(Movement m)
        {
            Board currentBoard = board!;

            if (m.Direction == Direction.RIGHT || m.Direction == Direction.LEFT)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (currentBoard.Cubes[i, m.Y] is FreezedCube)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (currentBoard.Cubes[m.X, i] is FreezedCube)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Applies a movement on the board and notifies observers.
        /// </summary>
        /// <param name="m">Movement to execute.</param>
        public void MoveCubes(Movement m)
        {
            if (board is null || !CheckMovement(m))
                return;

            Symbol symbol = game.CurrentPlayer.Symbol;

            if (m.Direction == Direction.RIGHT || m.Direction == Direction.LEFT)
                MoveHorizontal(m, symbol, m.Direction == Direction.RIGHT);
            else
                MoveVertical(m, symbol, m.Direction == Direction.DOWN);

            BoardChanged?.Invoke(board);
        }

        /// <summary>
        /// Switches the current player and notifies observers.
        /// </summary>
        public void SwitchPlayer()
        {
            game.CurrentPlayer = game.CurrentPlayer == game.Player1 ? game.Player2 : game.Player1;
            ActivePlayerChanged?.Invoke(game.CurrentPlayer);
        }

        /// <summary>
        /// Allows external callers to request that observers are notified of the current board state.
        /// </summary>
        public void NotifyBoardChanged()
        {
            BoardChanged?.Invoke(board!);
        }

        /// <summary>
        /// Moves a row horizontally and inserts the player's symbol.
        /// </summary>
        /// <param name="m">Movement describing the row and starting position.</param>
        /// <param name="s">Current player's symbol.</param>
        /// <param name="moveRight">true for a shift to the right; otherwise to the left.</param>
        private void MoveHorizontal(Movement m, Symbol s, bool moveRight)
        {
            Symbol[] row = new Symbol[5];
            for (int i = 0; i < 5; i++)
                row[i] = board!.Cubes[i, m.Y].Symbol;

            if (moveRight)
            {
                for (int i = m.X; i <= 3; i++)
                    row[i] = row[i + 1];
                row[4] = s;
            }
            else
            {
                for (int i = m.X; i >= 1; i--)
                    row[i] = row[i - 1];
                row[0] = s;
            }

            for (int i = 0; i < 5; i++)
                board!.Cubes[i, m.Y].Symbol = row[i];
        }

        /// <summary>
        /// Moves a column vertically and inserts the player's symbol.
        /// </summary>
        /// <param name="m">Movement describing the column and starting position.</param>
        /// <param name="s">Current player's symbol.</param>
        /// <param name="moveDown">true for a downward shift; otherwise upward.</param>
        private void MoveVertical(Movement m, Symbol s, bool moveDown)
        {
            Symbol[] col = new Symbol[5];
            for (int i = 0; i < 5; i++)
                col[i] = board!.Cubes[m.X, i].Symbol;

            if (moveDown)
            {
                for (int i = m.Y; i >= 1; i--)
                    col[i] = col[i - 1];
                col[0] = s;
            }
            else
            {
                for (int i = m.Y; i <= 3; i++)
                    col[i] = col[i + 1];
                col[4] = s;
            }

            for (int i = 0; i < 5; i++)
                board!.Cubes[m.X, i].Symbol = col[i];
        }
    }
}