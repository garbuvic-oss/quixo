using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Score
{
    /// <summary>
    /// Implements player score management.
    /// </summary>
    public class ScoreManager : IScoreManager
    {
        /// <summary>
        /// Increments the score of the specified player.
        /// </summary>
        /// <param name="player">Player whose score must be increased.</param>
        public void AddPoint(Player player)
        {
            player.Score++;
        }
    }
}