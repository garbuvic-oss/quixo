namespace Quixo.Resources.Pages;

public partial class InformationsPage : ContentPage
{
	public InformationsPage()
	{
		InitializeComponent();
	}

	private async void BtnRetour_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//HomePage");
	}
}