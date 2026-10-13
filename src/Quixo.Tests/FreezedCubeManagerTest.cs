using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Freeze;

namespace Quixo.Tests;

public class FreezedCubeManagerTest
{
    [Fact]
    public void SelectRandomCube_ShouldUseInjectedRandomValues()
    {
        var manager = new FreezedCubeManager(new Random(0));

        var cube = manager.SelectRandomCube();

        Assert.Equal(Symbol.NONE, cube.Symbol);
        Assert.InRange(cube.X, 0, 4);
        Assert.InRange(cube.Y, 0, 4);
    }
}