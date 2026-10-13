using Quixo.Lib.Enum;
using System.Runtime.Serialization;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents the Quixo board made of a 5x5 cube grid.
    /// </summary>
    [DataContract]
    public class Board
    {
        readonly Cube[,] cubes = new Cube[5, 5];

        /// <summary>
        /// Gets the board cube matrix.
        /// </summary>
        [DataMember]
        public Cube[,] Cubes
        {
            get { return cubes; }
        }

        /// <summary>
        /// Gets the symbol currently present at a board cell.
        /// </summary>
        /// <param name="x">Horizontal cell coordinate.</param>
        /// <param name="y">Vertical cell coordinate.</param>
        /// <returns>The symbol present at the requested cell.</returns>
        public Symbol GetSymbol(int x, int y)
        {
            return Cubes[x, y].Symbol;
        }
    }
}