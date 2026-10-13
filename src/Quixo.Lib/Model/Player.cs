using Quixo.Lib.Enum;
using System.Runtime.Serialization;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a player in a Quixo game.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Human))]
    [KnownType(typeof(Bot))]
    public class Player
    {
        readonly string name;
        readonly Symbol symbol;

        /// <summary>
        /// Gets the player name.
        /// </summary>
        [DataMember]
        public string Name => name;

        /// <summary>
        /// Gets the symbol assigned to the player.
        /// </summary>
        [DataMember]
        public Symbol Symbol => symbol;

        /// <summary>
        /// Gets or sets the player's current score.
        /// </summary>
        [DataMember]
        public int Score { get; set; }

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        /// <param name="name">Player name.</param>
        /// <param name="symbol">Symbol assigned to the player.</param>
        public Player(string name, Symbol symbol)
        {
            this.name = name;
            this.symbol = symbol;
        }

        /// <summary>
        /// Returns a string representation of the player.
        /// </summary>
        /// <returns>A string containing the player's name, symbol, and score.</returns>
        public override string ToString()
        {
            return $"Player: {Name}, Symbol: {Symbol}, Score: {Score}";
        }
    }
}