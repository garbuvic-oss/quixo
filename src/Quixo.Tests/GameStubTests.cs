using Quixo.Lib.Enum;
using Quixo.Stub;

namespace Quixo.Tests;

public class GameStubTests
{
    [Fact]
    public void CreateHumanVsBotGame_ShouldBuildExpectedPlayers()
    {
        var gameManager = GameStub.CreateHumanVsBotGame();

        Assert.Equal("Player", gameManager.Game.Player1.Name);
        Assert.Equal(Symbol.X, gameManager.Game.Player1.Symbol);
        Assert.Equal("AI", gameManager.Game.Player2.Name);
        Assert.Equal(Symbol.O, gameManager.Game.Player2.Symbol);
    }

    [Fact]
    public void CreateHumanVsHumanGame_ShouldBuildExpectedPlayers()
    {
        var gameManager = GameStub.CreateHumanVsHumanGame();

        Assert.Equal("Alice", gameManager.Game.Player1.Name);
        Assert.Equal("Bob", gameManager.Game.Player2.Name);
        Assert.Equal(Symbol.X, gameManager.Game.Player1.Symbol);
        Assert.Equal(Symbol.O, gameManager.Game.Player2.Symbol);
    }

    [Fact]
    public void CreateGameNearWin_ShouldPreloadDiagonalProgress()
    {
        var gameManager = GameStub.CreateGameNearWin();

        Assert.Equal(Symbol.X, gameManager.board.GetSymbol(0, 0));
        Assert.Equal(Symbol.X, gameManager.board.GetSymbol(1, 1));
        Assert.Equal(Symbol.X, gameManager.board.GetSymbol(2, 2));
        Assert.Equal(Symbol.X, gameManager.board.GetSymbol(3, 3));
        Assert.Equal(Symbol.NONE, gameManager.board.GetSymbol(4, 4));
    }
}