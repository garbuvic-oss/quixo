using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager;
using Quixo.Lib.Model;

namespace Quixo.Maui.ViewModels;

public class PlateauViewModel : INotifyPropertyChanged
{
    private GameManager? _gameManager;
    private BoardCellViewModel? _selected;
    private string _statusMessage = string.Empty;

    public ObservableCollection<BoardCellViewModel> BoardCells { get; } = new();

    public ICommand SelectCommand { get; }
    public ICommand MoveCommand { get; }

    public PlateauViewModel()
    {
        // default players
        _gameManager = new GameManager(new Human("Joueur 1", Symbol.X), new Human("Joueur 2", Symbol.O));
        InitializeBoard();

        SelectCommand = new Command<BoardCellViewModel>(Select);
        MoveCommand = new Command<Direction>(ExecuteMove);
    }

    private void InitializeBoard()
    {
        BoardCells.Clear();
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                var cube = _gameManager!.board.Cubes[x, y];
                var vm = new BoardCellViewModel(cube, x, y);
                BoardCells.Add(vm);
            }
        }
    }

    public string CurrentTurnText => _gameManager is null ? string.Empty : $"TOUR ACTUEL\n{_gameManager.Game.CurrentPlayer.Name}";

    public string ScoreText => _gameManager is null ? string.Empty : $"SCORE GLOBAL\n{_gameManager.Game.Player1.Score:D2} - {_gameManager.Game.Player2.Score:D2}";

    public string SelectedCubeText => _selected is null ? "Aucun cube sélectionné" : $"Cube sélectionné : ({_selected.X + 1}, {_selected.Y + 1})";

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    private void Select(BoardCellViewModel? cell)
    {
        if (cell is null) return;
        if (!cell.IsEnabled)
        {
            StatusMessage = "Ce cube ne peut pas être sélectionné.";
            return;
        }

        _selected = _selected == cell ? null : cell;
        StatusMessage = _selected is null ? "Sélection annulée." : $"Cube sélectionné : ({_selected.X + 1}, {_selected.Y + 1})";
        RefreshAllCells();
        OnPropertyChanged(nameof(SelectedCubeText));
        OnPropertyChanged(nameof(CurrentTurnText));
    }

    private void ExecuteMove(Direction dir)
    {
        if (_gameManager is null)
        {
            StatusMessage = "Jeu non initialisé.";
            return;
        }

        if (_selected is null)
        {
            StatusMessage = "Sélectionne un cube avant de jouer.";
            return;
        }

        var m = GameManager.CreateMove(_selected.X, _selected.Y, dir);
        if (!_gameManager.pManager.CheckMovement(m))
        {
            StatusMessage = "Mouvement invalide.";
            return;
        }

        _gameManager.PlayTurn(m);
        StatusMessage = $"Mouvement {dir} joué.";
        _selected = null;
        RefreshAllCells();
        OnPropertyChanged(nameof(CurrentTurnText));
        OnPropertyChanged(nameof(SelectedCubeText));
    }

    private void RefreshAllCells()
    {
        foreach (var c in BoardCells)
        {
            c.IsSelected = _selected == c;
            c.Refresh();
        }
        OnPropertyChanged(nameof(BoardCells));
        OnPropertyChanged(nameof(ScoreText));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}