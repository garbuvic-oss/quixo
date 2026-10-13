using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Placement;
using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Bot
{
    /// <summary>
    /// Provides move generation for a bot player.
    /// </summary>
    public class BotManager : IBotManager
    {
        private readonly PlacementManager? pManager;
        private readonly Random random;

        /// <summary>
        /// Calculates random movements until a valid move is found.
        /// </summary>
        /// <returns>A valid movement found randomly.</returns>
        public Movement CalculRandomMove()
        {
            if (pManager is null)
            {
                throw new InvalidOperationException("Placement manager is required.");
            }

            Movement m = new(0, 0, Direction.UP);
            bool isValid = false;

            while (!isValid)
            {
                Direction d;
                int x = random.Next(0, 5);
                int y = random.Next(0, 5);
                int z = random.Next(0, 4);

                if (z == 0)
                {
                    d = Direction.UP;
                }
                else if (z == 1)
                {
                    d = Direction.DOWN;
                }
                else if (z == 2)
                {
                    d = Direction.LEFT;
                }
                else
                {
                    d = Direction.RIGHT;
                }

                m = new(x, y, d);
                isValid = pManager.CheckMovement(m);
            }

            return m;
        }

        /// <summary>
        /// Initializes a new instance of the BotManager class.
        /// </summary>
        /// <param name="pManager">Placement manager used to validate moves.</param>
        /// <param name="random">Random generator used to create moves.</param>
        public BotManager(PlacementManager? pManager, Random? random = null)
        {
            this.pManager = pManager;
            this.random = random ?? Random.Shared;
        }
    }
}
