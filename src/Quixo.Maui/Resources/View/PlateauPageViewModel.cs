using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Quixo.Json;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager;
using Quixo.Lib.Model;

namespace Quixo.Resources.Pages;

public class PlateauPageViewModel : INotifyPropertyChanged
{
    readonly GameManager gameManager;
    readonly JsonSaveManager saveManager = new();
    Cube? selectedCube;

    public event PropertyChangedEventHandler? PropertyChanged;

    public GameManager GameManager => gameManager;
    
    public Game Game => gameManager.Game;
    
    public IReadOnlyList<Cube> FlatBoard => gameManager.FlatBoard;

    public ICommand SelectCubeCommand { get; }

    public Command MoveCommand { get; }

    public ICommand SaveAndQuitCommand { get; }

    public Cube? SelectedCube
    {
        get => selectedCube;
        set
        {
            if (ReferenceEquals(selectedCube, value))
            {
                return;
            }

            if (selectedCube is not null)
            {
                selectedCube.IsSelected = false;
            }

            selectedCube = value;

            if (selectedCube is not null)
            {
                selectedCube.IsSelected = true;
            }

            OnPropertyChanged(nameof(SelectedCube));
            MoveCommand.ChangeCanExecute();
        }
    }

    public PlateauPageViewModel(GameManager gameManager)
    {
        this.gameManager = gameManager;
        SelectCubeCommand = new Command<Cube>(SelectCube);
        MoveCommand = new Command<Direction>(ExecuteMove, CanExecuteMove);
        SaveAndQuitCommand = new Command(async () => await SaveAndQuitAsync());

        // subscribe to board and player changes to refresh bindings
        if (gameManager.pManager is not null)
        {
            gameManager.pManager.BoardChanged += _ => OnBoardChanged();
            gameManager.pManager.ActivePlayerChanged += _ => OnActivePlayerChanged();
        }
    }

    private void OnBoardChanged()
    {
        OnPropertyChanged(nameof(FlatBoard));
        OnPropertyChanged(nameof(Game));
    }

    private void OnActivePlayerChanged()
    {
        OnPropertyChanged(nameof(Game));
        MoveCommand.ChangeCanExecute();
    }

    void SelectCube(Cube? cube)
    {
        // Only allow selecting edge cubes that are empty or already owned by the current player
        if (cube is null)
        {
            SelectedCube = null;
            return;
        }

        bool isEdge = cube.X == 0 || cube.Y == 0 || cube.X == 4 || cube.Y == 4;
        bool canTakeCube = cube.Symbol == Symbol.NONE || cube.Symbol == gameManager.Game.CurrentPlayer.Symbol;

        if (!isEdge || !canTakeCube)
        {
            // ignore selection for non-edge or occupied cubes
            return;
        }

        SelectedCube = cube == selectedCube ? null : cube;
    }

    bool CanExecuteMove(Direction direction)
    {
        if (SelectedCube is null)
        {
            return false;
        }

        return gameManager.pManager.CheckMovement(GameManager.CreateMove(SelectedCube.X, SelectedCube.Y, direction));
    }

    void ExecuteMove(Direction direction)
    {
        if (SelectedCube is null)
        {
            return;
        }

        gameManager.PlayTurn(GameManager.CreateMove(SelectedCube.X, SelectedCube.Y, direction));
        SelectedCube = null;

        // If the next player is a bot, let it play immediately.
        if (!gameManager.RoundFinished)
        {
            gameManager.PlayBotTurnIfNeeded();
        }
    }

    async Task SaveAndQuitAsync()
    {
        try
        {
            string savePath = JsonSaveManager.GetDefaultSavePath();
            bool saved = saveManager.SaveGame(gameManager.Game, savePath);
            if (!saved)
            {
                Shell? alertShell = Shell.Current;
                if (alertShell is not null)
                {
                    await alertShell.DisplayAlertAsync("Sauvegarde", "Impossible de sauvegarder la partie.", "OK");
                }

                return;
            }

            Shell? shell = Shell.Current;
            if (shell is not null)
            {
                await shell.DisplayAlertAsync("Sauvegarde", $"Partie sauvegardée dans {savePath}", "OK");
            }

            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            Shell? shell = Shell.Current;
            if (shell is not null)
            {
                await shell.DisplayAlertAsync("Sauvegarde", ex.Message, "OK");
            }
        }
    }

    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
