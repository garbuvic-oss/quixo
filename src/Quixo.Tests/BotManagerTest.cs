using Quixo.Lib.Enum;
using Quixo.Lib.Manager;
using Quixo.Lib.Manager.Bot;
using Quixo.Lib.Manager.Placement;
using Quixo.Lib.Model;

namespace Quixo.Tests
{
    public class BotManagerTests
    {
        [Fact]
        public void CalculRandomMove_ShouldReturnNonNullMovement()
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act
            var move = gManager.bManager.CalculRandomMove();

            // Assert
            Assert.NotNull(move);
        }

        [Fact]
        public void CalculRandomMove_ShouldReturnValidCoordinates()
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act
            var move = gManager.bManager.CalculRandomMove();

            // Assert
            Assert.True(move.X >= 0 && move.X <= 4);
            Assert.True(move.Y >= 0 && move.Y <= 4);
        }

        [Fact]
        public void CalculRandomMove_ShouldReturnValidDirection()
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act
            var move = gManager.bManager.CalculRandomMove();

            // Assert
            Assert.IsType<Direction>(move.Direction);
        }

        public static TheoryData<Direction> GetAllPossibleDirections()
        {
            var data = new TheoryData<Direction>()
            {
                Direction.UP,
                Direction.DOWN,
                Direction.LEFT,
                Direction.RIGHT,
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(GetAllPossibleDirections))]
        public void CalculRandomMove_ShouldEventuallyAssignAllDirections(Direction expectedDir)
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act & Assert
            bool directionFound = false;
            for (int i = 0; i < 100; i++)
            {
                var move = gManager.bManager.CalculRandomMove();
                if (move.Direction == expectedDir)
                {
                    directionFound = true;
                    break;
                }
            }

            Assert.True(directionFound);
        }

        [Fact]
        public void CalculRandomMove_DirectionShouldBeDefined()
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act
            var move = gManager.bManager.CalculRandomMove();

            // Assert
            // On vérifie que le hasard a bien choisi une direction qui existe dans l'Enum
            Assert.True(Enum.IsDefined<Direction>(move.Direction));
        }

        [Fact]
        public void CalculRandomMove_ShouldHandleNullPlacementManager()
        {
            // Arrange
            var bManager = new Quixo.Lib.Manager.Bot.BotManager(null);

            // Act & Assert
            Assert.NotNull(bManager);
        }

        [Fact]
        public void CalculRandomMove_ShouldThrowWhenPlacementManagerIsNull()
        {
            var bManager = new Quixo.Lib.Manager.Bot.BotManager(null);

            Assert.Throws<InvalidOperationException>(() => bManager.CalculRandomMove());
        }

        [Fact]
        public void CalculRandomMove_ShouldWorkOnFullRangeOfRandomValues()
        {
            // Arrange
            Player p1 = new Player("Mathéo", Symbol.X);
            Player p2 = new Player("Emeric", Symbol.O);
            GameManager gManager = new GameManager(p1, p2);

            // Act
            List<Movement> moves = new List<Movement>();
            for (int i = 0; i < 50; i++)
            {
                moves.Add(gManager.bManager.CalculRandomMove());
            }

            // Assert
            Assert.All(moves, m =>
            {
                Assert.True(m.X >= 0 && m.X < 5);
                Assert.True(m.Y >= 0 && m.Y < 5);
                Assert.True(Enum.IsDefined<Direction>(m.Direction));
            });
        }

        [Fact]
        public void CalculRandomMove_TestNullCoalescingOperator()
        {
            // Arrange
            PlacementManager? pManager = null;
            Movement movement = new(0, 0, Direction.UP);

            // Act
            bool isValid = pManager?.CheckMovement(movement) ?? false;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CalculRandomMove_ShouldUseInjectedRandomAndReturnValidMove()
        {
            var gameManager = new GameManager(new Player("Mathéo", Symbol.X), new Player("Emeric", Symbol.O));
            var botManager = new BotManager((PlacementManager)gameManager.pManager, new Random(0));

            var movement = botManager.CalculRandomMove();

            Assert.InRange(movement.X, 0, 4);
            Assert.InRange(movement.Y, 0, 4);
            Assert.True(Enum.IsDefined<Direction>(movement.Direction));
        }
    }
}
