using Quixo.Lib.Enum;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a player controlled by artificial intelligence.
    /// </summary>
    public class Bot : Player
    {

        /// <summary>
        /// Initializes a new instance of the Bot class.
        /// </summary>
        /// <param name="name">Bot name.</param>
        /// <param name="symbole">Symbol assigned to the bot.</param>
        public Bot(string name, Symbol symbole) : base(name, symbole)
        { }
    }

}