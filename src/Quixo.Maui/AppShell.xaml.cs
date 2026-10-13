using Quixo.Resources.Pages;

namespace Quixo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("ConfigPage", typeof(ConfigPage));
        Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
        Routing.RegisterRoute("InformationsPage", typeof(InformationsPage));
        Routing.RegisterRoute("PlateauPage", typeof(PlateauPage));
        Routing.RegisterRoute("HomePage", typeof(HomePage));
    }
}
