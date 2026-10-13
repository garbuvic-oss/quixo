using Quixo.Lib.Manager;
using Quixo.Lib.Model;
using Quixo.Lib.Enum;

namespace Quixo;

public partial class App : Application
{
	// Expose a shared GameManager for pages to consume
	public GameManager? gameManager { get; set; }

	public App()
	{
		InitializeComponent();
		// Create a default game manager (two human players)
		gameManager = new GameManager(new Human("Joueur 1", Symbol.X), new Human("Joueur 2", Symbol.O));
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}