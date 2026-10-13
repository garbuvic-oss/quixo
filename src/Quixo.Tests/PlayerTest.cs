using Quixo.Lib.Enum;
using Quixo.Lib.Model;

namespace Quixo.Tests;

public class PlayerTest
{
    [Fact]
    public void ToString_ShouldIncludePlayerState()
    {
        var player = new Player("Alice", Symbol.X) { Score = 3 };

        Assert.Equal("Player: Alice, Symbol: X, Score: 3", player.ToString());
    }
}