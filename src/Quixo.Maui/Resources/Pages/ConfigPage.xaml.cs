using Quixo.Json;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager;
using Quixo.Lib.Model;

namespace Quixo.Resources.Pages;

public partial class ConfigPage : ContentPage
{
    public string PlayerOneName { get; set; } = "Joueur 1";
    public string PlayerTwoName { get; set; } = "Joueur 2";
    public bool IsPlayerTwoBot { get; set; } = false;

    public ConfigPage()
    {
        InitializeComponent();

        // Relie le XAML aux propriétés ci-dessus
        this.BindingContext = this;
    }

    private async void BtnLancer_Clicked(object sender, EventArgs e)
    {
        var app = Application.Current as App;
        if (app is not null)
        {
            Quixo.Lib.Model.Player p1 = new Quixo.Lib.Model.Human(PlayerOneName, Quixo.Lib.Enum.Symbol.X);
            Quixo.Lib.Model.Player p2;
            if (IsPlayerTwoBot)
                p2 = new Quixo.Lib.Model.Bot(PlayerTwoName, Quixo.Lib.Enum.Symbol.O);
            else
                p2 = new Quixo.Lib.Model.Human(PlayerTwoName, Quixo.Lib.Enum.Symbol.O);

            app.gameManager = new Quixo.Lib.Manager.GameManager(p1, p2);
        }

        await Shell.Current.GoToAsync("PlateauPage");
    }

    private async void BtnRetour_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void BtnCharger_Clicked(object sender, EventArgs e)
    {
        await LoadGameFromFileAsync();
    }

    private async Task LoadGameFromFileAsync()
    {
        try
        {
            JsonSaveManager jsonSaveManager = new();
            string savePath = JsonSaveManager.GetDefaultSavePath();
            Game loadedGame = jsonSaveManager.LoadGame(savePath);

            if (Application.Current is App app)
            {
                app.gameManager = new GameManager(loadedGame);
            }

            await Shell.Current.GoToAsync("PlateauPage");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Chargement impossible", ex.Message, "OK");
        }
    }
}