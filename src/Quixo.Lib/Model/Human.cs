using Quixo.Lib.Enum;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a human player.
    /// </summary>
    public class Human : Player
    {
        /// <summary>
        /// Initializes a new instance of the Human class.
        /// </summary>
        /// <param name="name">Human player name.</param>
        /// <param name="symbole">Symbol assigned to the human player.</param>
        public Human(string name, Symbol symbole) : base(name, symbole)
        { }
    }
}