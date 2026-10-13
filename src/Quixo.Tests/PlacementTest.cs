using System.Reflection;
using System.Runtime.CompilerServices;
using Quixo.Lib.Model;
using Quixo.Lib.Manager;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Grid;
using Quixo.Lib.Manager.Placement;
using Xunit;

namespace Quixo.Test;

public class GameManagerPlacementTests
{
    private static Board CreateInitializedBoard()
    {
        var board = new Board();
        _ = new GridManager(board);
        return board;
    }

    [Fact]
    public void PlacementManager_CheckMovement_ShouldRejectFrozenCubeOnHorizontalPath()
    {
        var gm = CreateGameManager();
        gm.board.Cubes[2, 1] = new FreezedCube(Symbol.O, 2, 1);

        var isValid = gm.pManager.CheckMovement(new Movement(0, 1, Direction.RIGHT));

        Assert.False(isValid);
    }

    [Fact]
    public void PlacementManager_CheckMovement_ShouldRejectFrozenCubeOnVerticalPath()
    {
        var gm = CreateGameManager();
        gm.board.Cubes[0, 2] = new FreezedCube(Symbol.O, 0, 2);

        var isValid = gm.pManager.CheckMovement(new Movement(0, 0, Direction.DOWN));

        Assert.False(isValid);
    }

    private static GameManager CreateGameManager()
    {
        return new GameManager(new Human("Alice", Symbol.X), new Human("Bob", Symbol.O));
    }

    // idk how to test the case where the board is null without using reflection, so here we are
    // idk why it works but it does, so I won't question it
    private static PlacementManager CreatePlacementManagerWithNullBoard()
    {
        var game = new Game(new Human("Alice", Symbol.X), new Human("Bob", Symbol.O), new Board());
        var gridManager = (GridManager)RuntimeHelpers.GetUninitializedObject(typeof(GridManager));
        var boardField = typeof(GridManager).GetField("<Board>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(boardField);
        boardField!.SetValue(gridManager, null);

        return new PlacementManager(game, gridManager);
    }

    [Fact]
    public void PlayTurn_ShouldUpdateBoardAndSwitchPlayer()
    {
        // Arrange
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Human("Bob", Symbol.O);
        var gm = new GameManager(p1, p2);


        // Act : Alice joue en (0,0) et pousse vers la gauche
        var move = new Movement(0, 0, Direction.LEFT);
        gm.PlayTurn(move);

        // AsserT
        Assert.Equal(Symbol.NONE, gm.board.GetSymbol(0, 0));

        // 2. Le joueur actuel doit avoir changé
        Assert.Equal(p2, gm.Game.CurrentPlayer);
    }

    [Fact]
    public void CheckMovement_ShouldReturnFalse_IfDirectionIsInvalid()
    {
        // Arrange
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Human("Bob", Symbol.O);
        var gm = new GameManager(p1, p2);

        // Act : Tenter de pousser vers le haut depuis le bord haut (Y=0)
        var invalidMove = new Movement(0, 0, Direction.UP);
        bool isValid = gm.pManager.CheckMovement(invalidMove);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void CheckMovement_ShouldReturnFalse_IfNotOnEdge()
    {
        // Arrange
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Human("Bob", Symbol.O);
        var gm = new GameManager(p1, p2);

        // Act : Tenter de jouer un cube au centre (2,2)
        var invalidMove = new Movement(2, 2, Direction.RIGHT);
        bool isValid = gm.pManager.CheckMovement(invalidMove);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void GridManager_ShouldPopulateResetAndSwitchFace()
    {
        var board = CreateInitializedBoard();
        var gridManager = new GridManager(board);

        board.Cubes[0, 0].Symbol = Symbol.X;
        gridManager.ResetBoard();

        Assert.True(GridManager.IsEdge(0, 0));
        Assert.False(GridManager.IsEdge(2, 2));
        Assert.Equal(Symbol.NONE, board.GetSymbol(0, 0));

        gridManager.SwitchFace(0, 4, Symbol.O);
        gridManager.SwitchFace(2, 2, Symbol.X);
        gridManager.SwitchFace(0, 4, Symbol.X);

        Assert.Equal(Symbol.O, board.GetSymbol(0, 4));
        Assert.Equal(Symbol.NONE, board.GetSymbol(2, 2));
    }

    [Fact]
    public void PlacementManager_CheckMovement_ShouldAcceptValidEdgeMove()
    {
        var gm = CreateGameManager();

        var isValid = gm.pManager.CheckMovement(new Movement(0, 1, Direction.RIGHT));

        Assert.True(isValid);
    }

    [Theory]
    [InlineData(0, 1, Direction.LEFT)]
    [InlineData(4, 1, Direction.RIGHT)]
    [InlineData(1, 0, Direction.UP)]
    [InlineData(1, 4, Direction.DOWN)]
    public void PlacementManager_CheckMovement_ShouldRejectInvalidEdgeDirections(int x, int y, Direction direction)
    {
        var gm = CreateGameManager();

        var isValid = gm.pManager.CheckMovement(new Movement(x, y, direction));

        Assert.False(isValid);
    }

    [Fact]
    public void PlacementManager_CheckMovement_ShouldRejectOccupiedCell()
    {
        var gm = CreateGameManager();
        gm.board.Cubes[0, 1].Symbol = Symbol.O;

        var isValid = gm.pManager.CheckMovement(new Movement(0, 1, Direction.RIGHT));

        Assert.False(isValid);
    }

    [Fact]
    public void PlacementManager_CheckMovement_ShouldReturnFalse_WhenBoardIsNull()
    {
        var placementManager = CreatePlacementManagerWithNullBoard();

        var isValid = placementManager.CheckMovement(new Movement(0, 0, Direction.RIGHT));

        Assert.False(isValid);
    }

    [Fact]
    public void PlacementManager_MoveCubes_ShouldShiftRowRightAndRaiseEvents()
    {
        var gm = CreateGameManager();
        Board? eventBoard = null;
        Player? eventPlayer = null;
        gm.pManager.BoardChanged += board => eventBoard = board;
        gm.pManager.ActivePlayerChanged += player => eventPlayer = player;

        gm.pManager.MoveCubes(new Movement(2, 0, Direction.RIGHT));
        gm.pManager.SwitchPlayer();
        gm.pManager.SwitchPlayer();

        Assert.Same(gm.board, eventBoard);
        Assert.Same(gm.Game.Player1, eventPlayer);
        Assert.Equal(Symbol.X, gm.board.GetSymbol(4, 0));
        Assert.Equal(Symbol.NONE, gm.board.GetSymbol(2, 0));
        Assert.Same(gm.Game.Player1, gm.Game.CurrentPlayer);
    }

    [Fact]
    public void PlacementManager_MoveCubes_ShouldShiftRowLeft()
    {
        var gm = CreateGameManager();

        gm.pManager.MoveCubes(new Movement(2, 4, Direction.LEFT));

        Assert.Equal(Symbol.X, gm.board.GetSymbol(0, 4));
        Assert.Equal(Symbol.NONE, gm.board.GetSymbol(2, 4));
    }

    [Fact]
    public void PlacementManager_MoveCubes_ShouldShiftColumnDown()
    {
        var gm = CreateGameManager();

        gm.pManager.MoveCubes(new Movement(0, 3, Direction.DOWN));

        Assert.Equal(Symbol.X, gm.board.GetSymbol(0, 0));
        Assert.Equal(Symbol.NONE, gm.board.GetSymbol(0, 4));
    }

    [Fact]
    public void PlacementManager_MoveCubes_ShouldShiftColumnUp()
    {
        var gm = CreateGameManager();

        gm.pManager.MoveCubes(new Movement(0, 1, Direction.UP));

        Assert.Equal(Symbol.X, gm.board.GetSymbol(0, 4));
        Assert.Equal(Symbol.NONE, gm.board.GetSymbol(0, 0));
    }

    [Fact]
    public void PlacementManager_MoveCubes_ShouldDoNothingWhenBoardIsNull()
    {
        var placementManager = CreatePlacementManagerWithNullBoard();
        var wasCalled = false;
        placementManager.BoardChanged += _ => wasCalled = true;

        placementManager.MoveCubes(new Movement(0, 0, Direction.RIGHT));

        Assert.False(wasCalled);
    }

} 