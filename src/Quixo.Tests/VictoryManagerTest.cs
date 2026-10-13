using Xunit;
using Quixo.Lib.Manager.Victory;
using Quixo.Lib.Model;
using Quixo.Lib.Enum;
using System.Collections.Generic;

namespace Quixo.Tests
{
    public class VictoryManagerTests
    {
        // Méthode utilitaire pour éviter les NullReferenceException
        // Elle prépare un plateau où chaque Cube est instancié
        private static Board CreateInitializedBoard()
        {
            Board board = new Board();

            // On parcourt la grille pour initialiser chaque Cube
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    board.Cubes[i, j] = new Cube(Symbol.NONE);
                }
            }
            return board;
        }

        // --- TESTS POUR LES LIGNES ---

        [Fact]
        public void CheckRow_ShouldReturnX_WhenRow0IsAllX()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[0, i].Symbol = Symbol.X;
            }
            VictoryManager vManager = new VictoryManager(board);
            Symbol winner = Symbol.NONE;
            vManager.GameFinished += symbol => winner = symbol;

            // Act
            Symbol result = vManager.CheckRow(0);

            // Assert
            Assert.Equal(Symbol.X, result);
            Assert.Equal(Symbol.X, winner);
        }

        [Fact]
        public void CheckRow_ShouldReturnX_WhenRow0IsAllX_WithoutSubscriber()
        {
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[0, i].Symbol = Symbol.X;
            }
            VictoryManager vManager = new VictoryManager(board);

            Symbol result = vManager.CheckRow(0);

            Assert.Equal(Symbol.X, result);
        }

        [Fact]
        public void CheckRow_ShouldReturnNone_WhenRowIsMixed()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            board.Cubes[0, 0].Symbol = Symbol.X;
            board.Cubes[0, 1].Symbol = Symbol.O;
            board.Cubes[0, 2].Symbol = Symbol.X;
            VictoryManager vManager = new VictoryManager(board);

            // Act
            Symbol result = vManager.CheckRow(0);

            // Assert
            Assert.Equal(Symbol.NONE, result);
        }

        // --- TESTS POUR LES COLONNES ---

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void CheckColumn_ShouldReturnO_WhenColumnIsAllO(int columnIndex)
        {
            // Arrange
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[i, columnIndex].Symbol = Symbol.O;
            }
            VictoryManager vManager = new VictoryManager(board);
            Symbol winner = Symbol.NONE;
            vManager.GameFinished += symbol => winner = symbol;

            // Act
            Symbol result = vManager.CheckColumn(columnIndex);

            // Assert
            Assert.Equal(Symbol.O, result);
            Assert.Equal(Symbol.O, winner);
        }

        [Fact]
        public void CheckColumn_ShouldReturnNone_WhenColumnIsEmpty()
        {
            Board board = CreateInitializedBoard();
            VictoryManager vManager = new VictoryManager(board);

            Symbol result = vManager.CheckColumn(2);

            Assert.Equal(Symbol.NONE, result);
        }

        [Fact]
        public void CheckColumn_ShouldReturnO_WhenColumnIsAllO_WithoutSubscriber()
        {
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[i, 0].Symbol = Symbol.O;
            }
            VictoryManager vManager = new VictoryManager(board);

            Symbol result = vManager.CheckColumn(0);

            Assert.Equal(Symbol.O, result);
        }

        // --- TESTS POUR LES DIAGONALES ---

        [Fact]
        public void CheckDiag_ShouldReturnX_WhenRightDiagonalIsAllX()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[i, i].Symbol = Symbol.X;
            }
            VictoryManager vManager = new VictoryManager(board);
            Symbol winner = Symbol.NONE;
            vManager.GameFinished += symbol => winner = symbol;

            // Act
            Symbol result = vManager.CheckDiag();

            // Assert
            Assert.Equal(Symbol.X, result);
            Assert.Equal(Symbol.X, winner);
        }

        [Fact]
        public void CheckDiag_ShouldReturnX_WhenRightDiagonalIsAllX_WithoutSubscriber()
        {
            Board board = CreateInitializedBoard();
            for (int i = 0; i < 5; i++)
            {
                board.Cubes[i, i].Symbol = Symbol.X;
            }
            VictoryManager vManager = new VictoryManager(board);

            Symbol result = vManager.CheckDiag();

            Assert.Equal(Symbol.X, result);
        }

        [Fact]
        public void CheckDiag_ShouldReturnO_WhenLeftDiagonalIsAllO()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            board.Cubes[0, 4].Symbol = Symbol.O;
            board.Cubes[1, 3].Symbol = Symbol.O;
            board.Cubes[2, 2].Symbol = Symbol.O;
            board.Cubes[3, 1].Symbol = Symbol.O;
            board.Cubes[4, 0].Symbol = Symbol.O;
            VictoryManager vManager = new VictoryManager(board);
            Symbol winner = Symbol.NONE;
            vManager.GameFinished += symbol => winner = symbol;

            // Act
            Symbol result = vManager.CheckDiag();

            // Assert
            Assert.Equal(Symbol.O, result);
            Assert.Equal(Symbol.O, winner);
        }

        [Fact]
        public void CheckDiag_ShouldReturnO_WhenLeftDiagonalIsAllO_WithoutSubscriber()
        {
            Board board = CreateInitializedBoard();
            board.Cubes[0, 4].Symbol = Symbol.O;
            board.Cubes[1, 3].Symbol = Symbol.O;
            board.Cubes[2, 2].Symbol = Symbol.O;
            board.Cubes[3, 1].Symbol = Symbol.O;
            board.Cubes[4, 0].Symbol = Symbol.O;
            VictoryManager vManager = new VictoryManager(board);

            Symbol result = vManager.CheckDiag();

            Assert.Equal(Symbol.O, result);
        }

        // --- TESTS AVEC MEMBERDATA ---

        public static TheoryData<int> GetEmptyRows()
        {
            var data = new TheoryData<int>
            {
                1,
                3,
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(GetEmptyRows))]
        public void CheckRow_ShouldReturnNone_WhenRowIsEmpty(int rowIndex)
        {
            // Arrange
            Board board = CreateInitializedBoard();
            VictoryManager vManager = new VictoryManager(board);

            // Act
            Symbol result = vManager.CheckRow(rowIndex);

            // Assert
            Assert.Equal(Symbol.NONE, result);
        }

        [Fact]
        public void CheckColumn_ShouldReturnNone_WhenColumnIsMixed()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            board.Cubes[0, 0].Symbol = Symbol.X;
            board.Cubes[1, 0].Symbol = Symbol.O; // Cas mixte
            VictoryManager vManager = new VictoryManager(board);

            // Act
            Symbol result = vManager.CheckColumn(0);

            // Assert
            Assert.Equal(Symbol.NONE, result);
        }

        [Fact]
        public void CheckDiag_ShouldReturnNone_WhenDiagonalsAreMixed()
        {
            // Arrange
            Board board = CreateInitializedBoard();
            // On remplit partiellement pour casser les deux diagonales
            board.Cubes[0, 0].Symbol = Symbol.X;
            board.Cubes[1, 1].Symbol = Symbol.O;
            board.Cubes[0, 4].Symbol = Symbol.X;
            board.Cubes[1, 3].Symbol = Symbol.O;
            VictoryManager vManager = new VictoryManager(board);

            // Act
            Symbol result = vManager.CheckDiag();

            // Assert
            Assert.Equal(Symbol.NONE, result);
        }

        [Fact]
        public void CheckDiag_ShouldReturnNone_WhenDiagonalsAreEmpty()
        {
            // Arrange
            Board board = CreateInitializedBoard(); // Tout est à Symbol.NONE
            VictoryManager vManager = new VictoryManager(board);

            // Act
            Symbol result = vManager.CheckDiag();

            // Assert
            Assert.Equal(Symbol.NONE, result);
        }
    }
}
