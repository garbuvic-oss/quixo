namespace Quixo.Resources.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

	private async void BtnDemarrer_Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("ConfigPage");
	}

	private async void BtnParametres_Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("SettingsPage");
	}

	private async void BtnInformations_Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("InformationsPage");
	}

    private void BtnQuitter_Clicked(object? sender, EventArgs e)
    {
        Application.Current?.Quit();
    }
}
