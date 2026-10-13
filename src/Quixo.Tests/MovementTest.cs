using Quixo.Lib.Enum;
using Quixo.Lib.Model;

namespace Quixo.Tests;

public class MovementTest
{
    [Fact]
    public void ToString_ShouldIncludeCoordinatesAndDirection()
    {
        var movement = new Movement(2, 3, Direction.LEFT);

        Assert.Equal("X: 2, Y: 3, Direction:LEFT", movement.ToString());
    }
}