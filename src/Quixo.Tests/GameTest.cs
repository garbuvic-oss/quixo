using Quixo.Lib.Enum;
using Quixo.Lib.Model;

namespace Quixo.Tests;

public class GameTest
{
    [Fact]
    public void SetCurrentPlayer_ShouldAcceptValidPlayerAndRejectInvalidOnes()
    {
        var player1 = new Human("Alice", Symbol.X);
        var player2 = new Human("Bob", Symbol.O);
        var game = new Game(player1, player2, new Board());
        var outsider = new Human("Charlie", Symbol.X);

        game.SetCurrentPlayer(player2);

        Assert.Same(player2, game.CurrentPlayer);
        Assert.NotNull(game.Board);
        Assert.Throws<ArgumentNullException>(() => game.SetCurrentPlayer(null!));
        Assert.Throws<ArgumentException>(() => game.SetCurrentPlayer(outsider));
    }
}