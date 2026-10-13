using Quixo.Lib.Manager;
using Quixo.Lib.Model;
using Quixo.Lib.Enum;
using Quixo.Stub;
using Quixo.Json;
 
static string GetSavePath()
{
    return JsonSaveManager.GetDefaultSavePath();
}

static int ReadInt(string message)
{
    while (true)
    {
        Console.Write(message);
        string? text = Console.ReadLine();

        if (int.TryParse(text, out int value))
        {
            return value;
        }

        Console.WriteLine("Entrée invalide. Réessayez...");
    }
}

static int DisplayMainMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("1. Player vs Player");
        Console.WriteLine("2. Player vs Bot");
        Console.WriteLine("3. Charger un scénario (STUB)");
        Console.WriteLine("4. Charger une partie");
        Console.Write("Choisir (1, 2, 3 ou 4): ");

        string? choiceText = Console.ReadLine();
        if (int.TryParse(choiceText, out int choice) && (choice == 1 || choice == 2 || choice == 3 || choice == 4))
        {
            return choice;
        }

        Console.WriteLine("Choix invalide. Appuyez sur Entrée...");
        Console.ReadLine();
    }
}

static int DisplayStubMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Scénarios stub:");
        Console.WriteLine("1. Humain vs Humain");
        Console.WriteLine("2. Humain vs Bot");
        Console.WriteLine("3. Partie presque gagnée");
        Console.Write("Choisir un scénario (1, 2 ou 3): ");

        string? choiceText = Console.ReadLine();
        if (int.TryParse(choiceText, out int choice) && choice >= 1 && choice <= 3)
        {
            return choice;
        }

        Console.WriteLine("Choix invalide. Appuyez sur Entrée...");
        Console.ReadLine();
    }
}

static void DelayForDisplay(int milliseconds)
{
    Thread.Sleep(milliseconds);
}

static Movement? GetCurrentMovement(GameManager gameManager, bool isVsBot, out bool wantToSave)
{
    wantToSave = false;
    if (isVsBot && gameManager.Game.CurrentPlayer is Bot)
    {
        Movement movement = gameManager.bManager.CalculRandomMove();
        Console.WriteLine($"Le bot joue: X={movement.X}, Y={movement.Y}, Direction={movement.Direction}");
        DelayForDisplay(1000);
        return movement;
    }

    return GetPlayerMovement(gameManager, out wantToSave);
}

static bool PromptForAnotherRound(Player player1, Player player2)
{
    Console.Clear();
    Console.WriteLine($"{player1.Name}: {player1.Score} | {player2.Name}: {player2.Score}");
    Console.WriteLine("\nVoulez-vous jouer une autre manche? (o/n)");
    return Console.ReadLine()?.ToLower() == "o";
}

static Movement? GetPlayerMovement(GameManager gameManager, out bool wantToSave)
{
    wantToSave = false;
    while (true)
    {
        Console.WriteLine("\n --- VOTRE TOUR ---");
        Console.WriteLine("Tapez 's' pour sauvegarder et quitter, ou appuyez sur n'importe quelle autre touche pour jouer : ");
        string? input = Console.ReadLine()?.Trim().ToLower();

        if (input == "s")
        {
            wantToSave = true;
            return null;
        }

        Console.WriteLine("\nEntrez votre mouvement : ");
        int x = ReadInt("X (0-4): ");
        int y = ReadInt("Y (0-4): ");

        Console.WriteLine("Direction:");
        Console.WriteLine(" [1] Vers le haut");
        Console.WriteLine(" [2] Vers le bas");
        Console.WriteLine(" [3] Vers la gauche");
        Console.WriteLine(" [4] Vers la droite");
        int dirChoice = ReadInt("Choisir (1-4): ");

        if (dirChoice >= 1 && dirChoice <= 4)
        {
            Direction dir = (Direction)(dirChoice - 1);
            Movement movement = GameManager.CreateMove(x, y, dir);

            if (gameManager.pManager.CheckMovement(movement))
            {
                return movement;
            }
        }

        Console.WriteLine("Entrée invalide. Réessayez...");
    }
}

// Retourne l'ensemble des symboles qui ont une ligne, colonne ou diagonale complète.
// Lit directement le plateau (board.GetSymbol) sans passer par vManager.
static HashSet<Symbol> GetCompletedLines(Board board)
{
    HashSet<Symbol> result = new();

    void Check(Func<int, Symbol> get)
    {
        Symbol first = get(0);
        if (first == Symbol.NONE) return;
        for (int i = 1; i < 5; i++)
        {
            if (get(i) != first) return;
        }
        result.Add(first);
    }

    for (int i = 0; i < 5; i++)
    {
        int line = i;
        Check(k => board.GetSymbol(k, line)); // ligne
        Check(k => board.GetSymbol(line, k)); // colonne
    }
    Check(k => board.GetSymbol(k, k));         // diagonale \
    Check(k => board.GetSymbol(k, 4 - k));     // diagonale /

    return result;
}

// Règle officielle du Quixo :
// - un seul symbole aligné : ce symbole gagne (même si c'est celui de l'adversaire)
// - les deux symboles alignés par le même coup : celui qui n'a PAS joué gagne
static Symbol ResolveWinner(Board board, Symbol mover)
{
    HashSet<Symbol> completed = GetCompletedLines(board);

    if (completed.Count == 0) return Symbol.NONE;
    if (completed.Count == 1) return completed.First();

    return mover == Symbol.X ? Symbol.O : Symbol.X;
}

static void DisplayBoard(Board board)
{
    Console.Clear();
    Console.WriteLine("\n     0   1   2   3   4");
    Console.WriteLine("   +---+---+---+---+---+");

    for (int y = 0; y < 5; y++)
    {
        Console.Write($" {y} |");
        for (int x = 0; x < 5; x++)
        {
            Symbol symbol = board.GetSymbol(x, y);
            string display = symbol switch
            {
                Symbol.X => "X",
                Symbol.O => "O",
                _ => " ",
            };

            Console.Write($" {display} |");
        }
        Console.WriteLine();
        Console.WriteLine("   +---+---+---+---+---+");
    }
}

static void RestoreGameState(GameManager targetManager, Game sourceGame)
{
    for (int x = 0; x < 5; x++)
    {
        for (int y = 0; y < 5; y++)
        {
            Cube sourceCube = sourceGame.Board.Cubes[x, y];
            targetManager.board.Cubes[x, y] = sourceCube is FreezedCube frozenCube
                ? new FreezedCube(frozenCube.Symbol, frozenCube.X, frozenCube.Y)
                : new Cube(sourceCube.Symbol);
        }
    }

    targetManager.Game.SetCurrentPlayer(sourceGame.CurrentPlayer);
}

static void PlayGame(Player player1, Player player2, bool isVsBot)
{
    while (true)
    {
        // true = la partie a été sauvegardée, on quitte la session
        bool saved = PlayRound(player1, player2, isVsBot);
        if (saved) return;

        if (!PromptForAnotherRound(player1, player2)) return;
    }
}

static void PlayGameWithManager(GameManager gameManager, bool isVsBot)
{
    while (true)
    {
        bool saved = PlayRoundWithManager(gameManager, isVsBot);
        if (saved) return;

        gameManager.gManager.ResetBoard();
        if (!PromptForAnotherRound(gameManager.Game.Player1, gameManager.Game.Player2)) return;
    }
}

static bool PlayRound(Player player1, Player player2, bool isVsBot)
{
    GameManager gameManager = new(player1, player2);
    return PlayRoundWithManager(gameManager, isVsBot);
}

// Retourne true si la partie a été sauvegardée (on quitte), false si la manche est terminée normalement.
static bool PlayRoundWithManager(GameManager gameManager, bool isVsBot)
{
    DisplayBoard(gameManager.board);
    gameManager.pManager.BoardChanged += DisplayBoard;

    bool gameActive = true;

    FreezedCube? frozenCube = null;
    int freezeTurnsLeft = 0;

    while (gameActive)
    {
        if (freezeTurnsLeft <= 0)
        {
            frozenCube = gameManager.freezeManager.SelectRandomCube();
            Symbol originalSymbol = gameManager.board.GetSymbol(frozenCube.X, frozenCube.Y);
            frozenCube.Symbol = originalSymbol;
            gameManager.board.Cubes[frozenCube.X, frozenCube.Y] = frozenCube;
            freezeTurnsLeft = 3;

            DisplayBoard(gameManager.board);
            Console.WriteLine($"\nUn nouveau cube a été gelé en X={frozenCube.X}, Y={frozenCube.Y} !");
        }
        else if (frozenCube != null)
        {
            int currentX = -1, currentY = -1;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (gameManager.board.Cubes[x, y] is FreezedCube)
                    {
                        currentX = x;
                        currentY = y;
                    }
                }
            }
            Console.WriteLine($"\nCube gelé actif en X={currentX}, Y={currentY} (Tours restants : {freezeTurnsLeft})");
        }

        Console.WriteLine($"\n{gameManager.Game.Player1.Name}: {gameManager.Game.Player1.Score} | {gameManager.Game.Player2.Name}: {gameManager.Game.Player2.Score}");
        Console.WriteLine($"\nTour de: {gameManager.Game.CurrentPlayer.Name} ({gameManager.Game.CurrentPlayer.Symbol})");

        Movement? movement = null;
        bool movementIsValid = false;
        while (!movementIsValid)
        {
            bool wantToSave;
            movement = GetCurrentMovement(gameManager, isVsBot, out wantToSave);

            if (wantToSave)
            {
                gameManager.pManager.BoardChanged -= DisplayBoard;
                try
                {
                    JsonSaveManager saver = new JsonSaveManager();
                    string path = GetSavePath();
                    bool saved = saver.SaveGame(gameManager.Game, path);

                    if (saved)
                    {
                        Console.WriteLine($"\n Partie sauvegardée avec succès dans '{path}' !");
                    }
                    else
                    {
                        Console.WriteLine($"\n Erreur lors de la sauvegarde : impossible d'écrire le fichier '{path}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n Erreur lors de la sauvegarde : {ex.Message}");
                }
                Console.WriteLine("Appuyez sur entrée pour revenir au menu principal");
                Console.ReadLine();
                return true;
            }

            movementIsValid = gameManager.pManager.CheckMovement(movement!);

            if (movementIsValid)
            {
                Player currentPlayer = gameManager.Game.CurrentPlayer;
                gameManager.PlayTurn(movement!);

                freezeTurnsLeft--;

                if (freezeTurnsLeft == 0)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            if (gameManager.board.Cubes[x, y] is FreezedCube fCube)
                            {
                                gameManager.board.Cubes[x, y] = new Cube(fCube.Symbol);
                            }
                        }
                    }
                    DelayForDisplay(1500);
                }

                Symbol winner = ResolveWinner(gameManager.board, currentPlayer.Symbol);
                if (winner != Symbol.NONE)
                {
                    Player winningPlayer = winner == gameManager.Game.Player1.Symbol
                        ? gameManager.Game.Player1
                        : gameManager.Game.Player2;

                    DisplayBoard(gameManager.board);
                    if (winningPlayer != currentPlayer)
                    {
                        Console.WriteLine($"\n {currentPlayer.Name} a aligné les symboles de l'adversaire !");
                    }
                    Console.WriteLine($"\n {winningPlayer.Name} a gagné!");
                    gameManager.sManager.AddPoint(winningPlayer);
                    Console.WriteLine("Appuyez sur Entrée pour continuer...");
                    Console.ReadLine();
                    gameActive = false;
                }
            }
            else
            {
                Console.WriteLine("\n Mouvement invalide ou bloqué par la glace ! Veuillez entrer un autre mouvement.");
                if (isVsBot && gameManager.Game.CurrentPlayer is Bot)
                {
                    DelayForDisplay(1000);
                }
            }
        }
    }

    gameManager.pManager.BoardChanged -= DisplayBoard;
    return false;
}

static (GameManager gameManager, bool isVsBot) SelectStubScenario()
{
    int choice = DisplayStubMenu();

    return choice switch
    {
        1 => (GameStub.CreateHumanVsHumanGame(), false),
        2 => (GameStub.CreateHumanVsBotGame(), true),
        3 => (GameStub.CreateGameNearWin(), false),
        _ => throw new InvalidOperationException("Scénario de stub invalide.")
    };
}

static string SelectName(int number)
{
    while (true)
    {
        Console.Write($"Nom du joueur {number} : ");
        string playerName = Console.ReadLine() ?? "";

        if (playerName.Length > 1)
        {
            return playerName;
        }

        Console.WriteLine("Le nom du joueur est trop court (minimum 2 caractères).");
    }
}

bool playAgain = true;

while (playAgain)
{
    int choice = DisplayMainMenu();

    if (choice == 1)
    {
        Console.Clear();
        string player1Name = SelectName(1);

        Console.Clear();
        string player2Name = SelectName(2);

        PlayGame(new Human(player1Name, Symbol.X), new Human(player2Name, Symbol.O), false);
    }
    else if (choice == 2)
    {
        Console.Clear();
        string player1Name = SelectName(1);

        Console.Clear();
        string player2Name = SelectName(2);

        PlayGame(new Human(player1Name, Symbol.X), new Bot(player2Name, Symbol.O), true);
    }
    else if (choice == 3)
    {
        var (stubGameManager, isVsBot) = SelectStubScenario();
        PlayGameWithManager(stubGameManager, isVsBot);
    }
    else if (choice == 4)
    {
        Console.Clear();
        string path = GetSavePath();
        try
        {
            JsonSaveManager loader = new JsonSaveManager();
            Game loadedGame = loader.LoadGame(path);

            GameManager loadedManager = new GameManager(loadedGame.Player1, loadedGame.Player2);
            RestoreGameState(loadedManager, loadedGame);
            bool isVsBot = loadedManager.Game.Player2 is Bot;

            Console.WriteLine("\n Partie chargée avec succès ! Lancement du jeu...");
            DelayForDisplay(1000);

            PlayGameWithManager(loadedManager, isVsBot);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"\n Aucun fichier de sauvegarde trouvé à l'emplacement '{path}'.");
            Console.WriteLine("Appuyez sur Entrée pour continuer...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n Erreur lors du chargement : {ex.Message}");
            Console.WriteLine("Appuyez sur Entrée pour continuer...");
            Console.ReadLine();
        }
    }

    Console.WriteLine("\nVoulez-vous revenir au menu principal? (o/n)");
    playAgain = Console.ReadLine()?.ToLower() == "o";
}