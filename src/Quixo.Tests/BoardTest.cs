using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Grid;
using Quixo.Lib.Model;
using Xunit;

namespace Quixo.Tests
{
    public class BoardTests
    {
        /// <summary>
        /// Méthode utilitaire pour créer un board prêt à l'emploi
        /// avec ses cubes initialisés par le GridManager.
        /// </summary>
        private static Board CreateInitializedBoard()
        {
            var board = new Board();
            // Le constructeur du GridManager appelle automatiquement PopulateBoard
            var manager = new GridManager(board);
            return board;
        }

        [Fact]
        public void Board_ShouldBePopulated_WhenGridManagerIsCreated()
        {
            // Arrange & Act
            var board = CreateInitializedBoard();

            // Assert
            // On vérifie qu'aucun cube n'est null dans la grille
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Assert.NotNull(board.Cubes[x, y]);
                }
            }
        }

        [Fact]
        public void GetSymbol_ShouldReturnCorrectSymbol_WhenCubeIsModified()
        {
            // Arrange
            var board = CreateInitializedBoard();
            int x = 2;
            int y = 3;

            // On modifie manuellement un cube pour le test
            board.Cubes[x, y].Symbol = Symbol.X;

            // Act
            var symbol = board.GetSymbol(x, y);

            // Assert
            Assert.Equal(Symbol.X, symbol);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 4)]
        public void GetSymbol_ShouldReturnNone_ByDefaultForBoundaries(int x, int y)
        {
            // Arrange
            var board = CreateInitializedBoard();

            // Act
            var symbol = board.GetSymbol(x, y);

            // Assert
            // On vérifie que PopulateBoard a bien mis Symbol.NONE partout
            Assert.Equal(Symbol.NONE, symbol);
        }

        [Fact]
        public void GetSymbol_ShouldThrowException_WhenIndexIsOutOfBounds()
        {
            // Arrange
            var board = CreateInitializedBoard();

            // Act & Assert
            Assert.Throws<System.IndexOutOfRangeException>(() => board.GetSymbol(5, 0));
        }
    }
}