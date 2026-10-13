namespace Quixo.Resources.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	private async void BtnEnregistrer_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//HomePage");
	}
}