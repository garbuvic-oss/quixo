using Quixo.Lib.Enum;
using Quixo.Lib.Model;
using Quixo.Lib.Manager;

namespace Quixo.Tests;

public class GameManagerTests
{
    [Fact]
    public void CreateMove_ShouldCreateExpectedMovement()
    {
        var movement = GameManager.CreateMove(2, 3, Direction.RIGHT);

        Assert.Equal(2, movement.X);
        Assert.Equal(3, movement.Y);
        Assert.Equal(Direction.RIGHT, movement.Direction);
    }

    [Fact]
    public void PlayTurn_ShouldShiftRowAndSwitchPlayer()
    {
        var gameManager = new GameManager(new Human("Alice", Symbol.X), new Human("Bob", Symbol.O));

        gameManager.PlayTurn(GameManager.CreateMove(0, 1, Direction.RIGHT));

        Assert.Equal(Symbol.X, gameManager.board.Cubes[4, 1].Symbol);
        Assert.Equal(Symbol.NONE, gameManager.board.Cubes[0, 1].Symbol);
        Assert.Same(gameManager.Game.Player2, gameManager.Game.CurrentPlayer);
        Assert.False(gameManager.RoundFinished);
    }

    [Fact]
    public void FlatBoard_ShouldExposeBoardInRowMajorOrder()
    {
        var gameManager = new GameManager(new Human("Alice", Symbol.X), new Human("Bob", Symbol.O));

        var flatBoard = gameManager.FlatBoard;

        Assert.Equal(25, flatBoard.Count);
        Assert.Same(gameManager.board.Cubes[0, 0], flatBoard[0]);
        Assert.Same(gameManager.board.Cubes[4, 4], flatBoard[^1]);
    }
}