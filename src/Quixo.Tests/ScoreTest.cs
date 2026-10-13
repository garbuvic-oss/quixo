using Quixo.Lib.Model;
using Quixo.Lib.Manager;
using Quixo.Lib.Enum;
using Xunit;

namespace Quixo.Tests;

public class GameManagerScoreTests
{
    [Fact]
    public void AddPoint_ViaGameManager_ShouldUpdatePlayerScore()
    {
        // Arrange : Initialisation via le GameManager
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Bot("Bob", Symbol.O);
        var gm = new GameManager(p1, p2);

        // Act : Utilisation du sManager exposé par le GameManager
        gm.sManager.AddPoint(p1);

        // Assert : Vérification que le score du joueur dans l'objet Game a changé
        Assert.Equal(1, gm.Game.Player1.Score);
    }

    [Fact]
    public void ScoreManager_ShouldDistinguishBetweenPlayers()
    {
        // Arrange
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Human("Bob", Symbol.O);
        var gm = new GameManager(p1, p2);

        // Act
        gm.sManager.AddPoint(p2);
        gm.sManager.AddPoint(p2);

        // Assert
        Assert.Equal(0, p1.Score);
        Assert.Equal(2, p2.Score);
    }

    [Fact]
    public void GameRestart_ScoreShouldPersistIfPlayersAreReused()
    {
        // Arrange
        var p1 = new Human("Alice", Symbol.X);
        var p2 = new Human("Bob", Symbol.O);
        var gm1 = new GameManager(p1, p2);
        gm1.sManager.AddPoint(p1);

        // Act : Simulation d'une nouvelle partie avec les mêmes instances de joueurs
        var gm2 = new GameManager(p1, p2);

        // Assert : Le score est stocké dans l'objet Player, il doit donc persister
        Assert.Equal(1, gm2.Game.Player1.Score);
    }
}