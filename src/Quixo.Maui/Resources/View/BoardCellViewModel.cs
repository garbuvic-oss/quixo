using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using Quixo.Lib.Model;
using Quixo.Lib.Enum;

namespace Quixo.Maui.ViewModels;

public class BoardCellViewModel : INotifyPropertyChanged
{
    private Cube? _cube;
    private string _displayText = string.Empty;
    private Color _background = Colors.Transparent;
    private Color _textColor = Colors.Black;
    private bool _isEnabled;
    private bool _isSelected;

    public BoardCellViewModel(Cube cube, int x, int y)
    {
        X = x;
        Y = y;
        AttachCube(cube);
    }

    public int X { get; }
    public int Y { get; }

    public string DisplayText
    {
        get => _displayText;
        private set { _displayText = value; OnPropertyChanged(); }
    }

    public Color BackgroundColor
    {
        get => _background;
        private set { _background = value; OnPropertyChanged(); }
    }

    public Color TextColor
    {
        get => _textColor;
        private set { _textColor = value; OnPropertyChanged(); }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        private set { _isEnabled = value; OnPropertyChanged(); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            // when selected, show a red background; otherwise transparent
            BackgroundColor = _isSelected ? Colors.Red : Colors.Transparent;
            OnPropertyChanged();
        }
    }

    private void AttachCube(Cube cube)
    {
        _cube = cube;
        _cube.PropertyChanged += Cube_PropertyChanged;
        Refresh();
    }

    private void Cube_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Cube.Symbol) || string.IsNullOrEmpty(e.PropertyName))
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (_cube is null) return;
        var symbol = _cube.Symbol;
        DisplayText = symbol == Symbol.NONE ? string.Empty : symbol.ToString();
        // By default, enable only edge cells that are empty (local check)
        bool isEdge = X == 0 || Y == 0 || X == 4 || Y == 4;
        IsEnabled = isEdge && symbol == Symbol.NONE;
        // default: transparent background and dark text for empty cells
        if (!_isSelected)
        {
            BackgroundColor = Colors.Transparent;
            TextColor = Color.FromArgb("#374151");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}