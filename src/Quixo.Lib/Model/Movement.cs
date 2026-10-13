using Quixo.Lib.Enum;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a move played on the board.
    /// </summary>
    public class Movement
    {
        /// <summary>
        /// Gets the horizontal coordinate of the selected cube.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the vertical coordinate of the selected cube.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets the movement direction.
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Initializes a new instance of the Movement class.
        /// </summary>
        /// <param name="x">Horizontal coordinate of the selected cube.</param>
        /// <param name="y">Vertical coordinate of the selected cube.</param>
        /// <param name="d">Movement direction.</param>
        public Movement(int x, int y, Direction d)
        {
            X = x;
            Y = y;
            Direction = d;
        }

        /// <summary>
        /// Returns a string representation of the movement.
        /// </summary>
        /// <returns>A string containing coordinates and direction.</returns>
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Direction:{Direction}";
        }
    }
}