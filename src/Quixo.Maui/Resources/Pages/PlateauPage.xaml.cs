using System;
using Microsoft.Maui.Controls;
using Quixo.Lib.Manager;

namespace Quixo.Resources.Pages;

public partial class PlateauPage : ContentPage
{
    public PlateauPage()
    {
        InitializeComponent();
        BindViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindViewModel();
    }

    private async void BtnAbandonner_Clicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    void BindViewModel()
    {
        GameManager? gm = (Application.Current as App)?.gameManager;
        BindingContext = new PlateauPageViewModel(gm ?? throw new InvalidOperationException("GameManager is not initialized."));
    }
}