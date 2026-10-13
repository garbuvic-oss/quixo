using Quixo.Lib.Enum;
using Quixo.Lib.Model;
using Quixo.Lib.Manager.Freeze;
using System.ComponentModel;

namespace Quixo.Tests;

public class CubeTest
{
    [Fact]
    public void ToString_ShouldIncludeCubeSymbol()
    {
        Cube cube = new(Symbol.O);

        Assert.Equal("Symbol: O", cube.ToString());
    }

    [Fact]
    public void FreezedCube_ShouldStoreSymbolAndCoordinates()
    {
        var cube = new FreezedCube(Symbol.X, 2, 4);

        Assert.Equal(Symbol.X, cube.Symbol);
        Assert.Equal(2, cube.X);
        Assert.Equal(4, cube.Y);
    }

    [Fact]
    public void Cube_ShouldExposeDisplaySymbolAndRaisePropertyChanged()
    {
        var cube = new Cube(Symbol.NONE);
        List<string?> changedProperties = [];

        cube.PropertyChanged += (_, args) => changedProperties.Add(args.PropertyName);

        Assert.Equal(string.Empty, cube.DisplaySymbol);

        cube.Symbol = Symbol.O;

        Assert.Equal("O", cube.DisplaySymbol);
        Assert.Contains(nameof(Cube.Symbol), changedProperties);
        Assert.Contains(nameof(Cube.DisplaySymbol), changedProperties);
    }

    [Fact]
    public void Cube_ShouldTrackSelectionState()
    {
        var cube = new Cube(Symbol.X);

        Assert.False(cube.IsSelected);

        cube.IsSelected = true;

        Assert.True(cube.IsSelected);
        Assert.False(cube.IsFrozen);
    }
}