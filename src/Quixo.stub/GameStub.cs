using Quixo.Lib.Model;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager;

namespace Quixo.Stub
{
    /// <summary>
    /// Provides stub data for testing and development of a Quixo game.
    /// </summary>
    public static class GameStub
    {
        /// <summary>
        /// Creates a sample Quixo game with a human player (X) against a bot (O).
        /// </summary>
        /// <returns>A GameManager instance with stub data.</returns>
        public static GameManager CreateHumanVsBotGame()
        {
            var human = new Human("Player", Symbol.X);
            var bot = new Bot("AI", Symbol.O);

            return new GameManager(human, bot);
        }

        /// <summary>
        /// Creates a sample Quixo game with two human players.
        /// </summary>
        /// <returns>A GameManager instance with stub data.</returns>
        public static GameManager CreateHumanVsHumanGame()
        {
            var player1 = new Human("Alice", Symbol.X);
            var player2 = new Human("Bob", Symbol.O);

            return new GameManager(player1, player2);
        }

        /// <summary>
        /// Creates a game manager where player 1 (X) is about to win.
        /// </summary>
        /// <returns>A GameManager instance configured with a near-winning state.</returns>
        public static GameManager CreateGameNearWin()
        {
            var player1 = new Human("Champion", Symbol.X);
            var player2 = new Human("Challenger", Symbol.O);
            
            var gameManager = new GameManager(player1, player2);

            for (int i = 0; i < 4; i++)
            {
                gameManager.board.Cubes[i, i].Symbol = Symbol.X;
            }

            return gameManager;
        }
    }
}
