using System;
using Quixo.Lib.Model;
using Quixo.Lib.Enum;

namespace Quixo.Lib.Manager.Grid
{
    /// <summary>
    /// Handles initialization and basic operations on the board grid.
    /// </summary>
    public class GridManager : IGridManager
    {
        /// <summary>
        /// Gets the board managed by this manager.
        /// </summary>
        public Board Board { get; }

        /// <summary>
        /// Indicates whether a cell is located on the board edge.
        /// </summary>
        /// <param name="x">Horizontal cell coordinate.</param>
        /// <param name="y">Vertical cell coordinate.</param>
        /// <returns>true if the cell is on an edge; otherwise, false.</returns>
        public static bool IsEdge(int x, int y)
        {
            if (x == 0 || y == 0 || x == 4 || y == 4)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures the board grid is initialized and that cube coordinates match the matrix position.
        /// </summary>
        private void InitializeBoard()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    var c = Board.Cubes[x, y] ?? new Cube(Symbol.NONE);
                    c.X = x;
                    c.Y = y;
                    Board.Cubes[x, y] = c;
                }
            }
        }

        /// <summary>
        /// Resets the board by filling it with empty cubes.
        /// </summary>
        public void ResetBoard()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Board.Cubes[x, y] = new Cube(Symbol.NONE)
                    {
                        X = x,
                        Y = y
                    };
                }
            }
        }

        /// <summary>
        /// Places a player's symbol on an edge cell when that cell is empty.
        /// </summary>
        /// <param name="x">Horizontal coordinate of the target cell.</param>
        /// <param name="y">Vertical coordinate of the target cell.</param>
        /// <param name="symbol">Symbol to place on the cell.</param>
        public void SwitchFace(int x, int y, Symbol symbol)
        {
            if (Board.Cubes[x, y].Symbol == Symbol.NONE && IsEdge(x, y))
            {
                Board.Cubes[x, y].Symbol = symbol;
            }
        }

        /// <summary>
        /// Initializes a new instance of the GridManager class.
        /// </summary>
        /// <param name="board">Board to manage.</param>
        public GridManager(Board board)
        {
            Board = board;
            InitializeBoard();
        }
    }
}