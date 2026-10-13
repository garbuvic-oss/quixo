using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Bot
{
    /// <summary>
    /// Defines move generation operations for a bot.
    /// </summary>
    public interface IBotManager
    {
        /// <summary>
        /// Calculates a random valid movement.
        /// </summary>
        /// <returns>A randomly selected valid movement.</returns>
        public Movement CalculRandomMove();
    }
}