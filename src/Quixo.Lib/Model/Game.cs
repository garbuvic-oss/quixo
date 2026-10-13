using System.Runtime.Serialization;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a Quixo game with its players, board, and active player.
    /// </summary>
    [DataContract]
    public class Game
    {
        [DataMember(Name = "players")]
        Player[] players { get; set; }

        /// <summary>
        /// Gets the player who must play the next move.
        /// </summary>
        [DataMember]
        public Player CurrentPlayer { get; internal set; }

        readonly Board board;

        /// <summary>
        /// Gets the first player in the game.
        /// </summary>
        [DataMember]
        public Player Player1 => players[0];

        /// <summary>
        /// Gets the second player in the game.
        /// </summary>
        [DataMember]
        public Player Player2 => players[1];

        /// <summary>
        /// Gets the board associated with the game.
        /// </summary>
        [DataMember(Name = "board")]
        public Board Board => board;

        /// <summary>
        /// Explicitly sets the current player.
        /// </summary>
        /// <param name="player">Player to set as current player.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="player"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="player"/> does not belong to this game.</exception>
        public void SetCurrentPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player), "Player cannot be null.");
            if (!players.Contains(player)) throw new ArgumentException("Player must be one of the game players.", nameof(player));
            CurrentPlayer = player;
        }

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        /// <param name="player1">First player.</param>
        /// <param name="player2">Second player.</param>
        /// <param name="board">Board used for the game.</param>
        public Game(Player player1, Player player2, Board board)
        {
            players = [player1, player2];
            CurrentPlayer = player1;
            this.board = board;
        }
    }
}