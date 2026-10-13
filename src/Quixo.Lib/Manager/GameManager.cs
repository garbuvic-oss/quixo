using System.Collections.Generic;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Bot;
using Quixo.Lib.Manager.Freeze;
using Quixo.Lib.Manager.Grid;
using Quixo.Lib.Manager.Placement;
using Quixo.Lib.Manager.Score;
using Quixo.Lib.Manager.Victory;
using Quixo.Lib.Model;

namespace Quixo.Lib.Manager
{
    /// <summary>
    /// Orchestrates the different managers required for a Quixo game.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// Game board shared by the managers.
        /// </summary>
        public readonly Board board;

        /// <summary>
        /// Current game instance.
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// Grid manager.
        /// </summary>
        public readonly IGridManager gManager;

        /// <summary>
        /// Victory manager.
        /// </summary>
        public readonly IVictoryManager vManager;

        /// <summary>
        /// Bot manager.
        /// </summary>
        public readonly IBotManager bManager;

        /// <summary>
        /// Placement manager.
        /// </summary>
        public readonly IPlacementManager pManager;

        /// <summary>
        /// Score manager.
        /// </summary>
        public readonly IScoreManager sManager;

        /// <summary>
        /// Freeze manager.
        /// </summary>
        public readonly FreezedCubeManager freezeManager;

        /// <summary>
        /// Indicates whether the current round has just finished.
        /// </summary>
        public bool RoundFinished { get; private set; }

        public GameManager(Player p1, Player p2) : this(new Game(p1, p2, new Board()))
        {
        }

        public GameManager(Game game)
        {
            ArgumentNullException.ThrowIfNull(game);

            board = game.Board;
            Game = game;
            gManager = new GridManager(board);
            pManager = new PlacementManager(Game, (GridManager)gManager);
            vManager = new VictoryManager(board);
            bManager = new BotManager((PlacementManager)pManager);
            sManager = new ScoreManager();
            freezeManager = new FreezedCubeManager();

            // Subscribe to victory events to update scores and reset the board
            vManager.GameFinished += OnGameFinished;
        }

        private void OnGameFinished(Quixo.Lib.Enum.Symbol winnerSymbol)
        {
            RoundFinished = true;

            // Award point to the corresponding player
            if (Game.Player1.Symbol == winnerSymbol)
            {
                sManager.AddPoint(Game.Player1);
            }
            else if (Game.Player2.Symbol == winnerSymbol)
            {
                sManager.AddPoint(Game.Player2);
            }

            // Reset the board
            gManager.ResetBoard();

            // Restart the next round from player 1
            Game.CurrentPlayer = Game.Player1;

            // Notify observers that the board changed
            (pManager as PlacementManager)?.NotifyBoardChanged();
        }

        /// <summary>
        /// Gets the board flattened as a read-only list (row-major order) for UI binding.
        /// </summary>
        public IReadOnlyList<Cube> FlatBoard
        {
            get
            {
                var list = new List<Cube>(25);
                for (int y = 0; y < 5; y++)
                    for (int x = 0; x < 5; x++)
                        list.Add(board.Cubes[x, y]);
                return list;
            }
        }

        /// <summary>
        /// Builds a movement from coordinates and a direction.
        /// </summary>
        /// <param name="x">Horizontal coordinate of the selected cube.</param>
        /// <param name="y">Vertical coordinate of the selected cube.</param>
        /// <param name="direction">Push direction.</param>
        /// <returns>The corresponding movement instance.</returns>
        static public Movement CreateMove(int x, int y, Direction direction)
        {
            return new Movement(x, y, direction);
        }

        /// <summary>
        /// Executes a full game turn and switches to the next player.
        /// </summary>
        /// <param name="movement">Movement to play during the turn.</param>
        public void PlayTurn(Movement movement)
        {
            RoundFinished = false;

            // Apply the movement
            pManager.MoveCubes(movement);

            // After the move, check victory conditions related to the changed row/column/diagonals.
            // Check the row or column affected, then diagonals (both) as they may also be completed.
            if (movement.Direction == Direction.LEFT || movement.Direction == Direction.RIGHT)
            {
                // horizontal movement modifies the row at Y -> check that row
                vManager.CheckColumn(movement.Y);
            }
            else
            {
                // vertical movement modifies the column at X -> check that column
                vManager.CheckRow(movement.X);
            }

            // Always check diagonals as well (may have been completed by the move)
            vManager.CheckDiag();

            // If the round just ended, do not switch players.
            if (RoundFinished)
            {
                return;
            }

            // Finally switch player if the game hasn't been reset by OnGameFinished
            pManager.SwitchPlayer();
        }

        /// <summary>
        /// Plays the next bot move if the current player is a bot and the round is still active.
        /// </summary>
        public void PlayBotTurnIfNeeded()
        {
            if (RoundFinished || Game.CurrentPlayer is not Quixo.Lib.Model.Bot || bManager is null)
            {
                return;
            }

            var botMove = bManager.CalculRandomMove();
            PlayTurn(botMove);
        }

        
    }
}