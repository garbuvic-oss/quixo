using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Score
{
    /// <summary>
    /// Defines score management operations.
    /// </summary>
    public interface IScoreManager
    {
        /// <summary>
        /// Adds one point to the specified player.
        /// </summary>
        /// <param name="player">Player who receives the point.</param>
        public void AddPoint(Player player);
    }
}