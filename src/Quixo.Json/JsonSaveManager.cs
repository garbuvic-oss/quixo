using System.Text.Json;
using Quixo.Lib.Enum;
using Quixo.Lib.Manager.Persistence;
using Quixo.Lib.Model;

namespace Quixo.Json
{
    public class JsonSaveManager : ILoadManager, ISaveManager
    {
        static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true
        };

        public static string GetDefaultSavePath()
        {
            string rootDirectory = FindRepositoryRoot();
            string saveDirectory = Path.Combine(rootDirectory, "save");
            Directory.CreateDirectory(saveDirectory);

            return Path.Combine(saveDirectory, "partie_quixo.json");
        }

        static string FindRepositoryRoot()
        {
            DirectoryInfo? currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

            while (currentDirectory != null)
            {
                bool hasSrcDirectory = Directory.Exists(Path.Combine(currentDirectory.FullName, "src"));
                bool hasDocsDirectory = Directory.Exists(Path.Combine(currentDirectory.FullName, "docs"));

                if (hasSrcDirectory && hasDocsDirectory)
                {
                    return currentDirectory.FullName;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return AppContext.BaseDirectory;
        }

        public Game LoadGame(string Path)
        {
            if (!File.Exists(Path))
            {
                throw new FileNotFoundException("Le fichier de sauvegarde n'existe pas.");
            }

            if (new FileInfo(Path).Length == 0)
            {
                throw new InvalidDataException($"Le fichier de sauvegarde '{Path}' est vide ou a été tronqué.");
            }

            string json = File.ReadAllText(Path);

            try
            {
                GameSaveData? loadedData = JsonSerializer.Deserialize<GameSaveData>(json, SerializerOptions);
                return loadedData?.ToGame() ?? throw new InvalidDataException($"Le fichier '{Path}' ne contient pas une sauvegarde Quixo valide.");
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException($"Le fichier '{Path}' n'est pas une sauvegarde Quixo JSON valide.", ex);
            }
        }

        public bool SaveGame(Game game, string Path)
        {
            if (game == null)
            {
                return false;
            }

            string tempPath = Path + ".tmp";

            try
            {
                string? directory = System.IO.Path.GetDirectoryName(Path);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                GameSaveData data = GameSaveData.FromGame(game);
                string json = JsonSerializer.Serialize(data, SerializerOptions);
                File.WriteAllText(tempPath, json);

                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }

                File.Move(tempPath, Path);
                return true;
            }
            catch
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                return false;
            }
        }

        public static string SerializeGame(Game game)
        {
            GameSaveData data = GameSaveData.FromGame(game);
            return JsonSerializer.Serialize(data, SerializerOptions);
        }

        private sealed record GameSaveData(PlayerData[] Players, int CurrentPlayerIndex, CubeData[][] Board)
        {
            public static GameSaveData FromGame(Game game)
            {
                PlayerData[] players =
                [
                    PlayerData.FromPlayer(game.Player1),
                    PlayerData.FromPlayer(game.Player2)
                ];

                int currentPlayerIndex = ReferenceEquals(game.CurrentPlayer, game.Player2) ? 1 : 0;

                CubeData[][] board = new CubeData[5][];
                for (int x = 0; x < 5; x++)
                {
                    board[x] = new CubeData[5];
                    for (int y = 0; y < 5; y++)
                    {
                        Cube cube = game.Board.Cubes[x, y];
                        bool isFrozen = cube is FreezedCube;
                        board[x][y] = new CubeData(cube.Symbol, isFrozen, isFrozen ? ((FreezedCube)cube).X : x, isFrozen ? ((FreezedCube)cube).Y : y);
                    }
                }

                return new GameSaveData(players, currentPlayerIndex, board);
            }

            public Game ToGame()
            {
                Player player1 = Players[0].ToPlayer();
                Player player2 = Players[1].ToPlayer();
                Board board = new Board();

                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        CubeData cubeData = Board[x][y];
                        board.Cubes[x, y] = cubeData.IsFrozen
                            ? new FreezedCube(cubeData.Symbol, cubeData.X, cubeData.Y)
                            : new Cube(cubeData.Symbol);
                    }
                }

                Game game = new Game(player1, player2, board);
                game.SetCurrentPlayer(CurrentPlayerIndex == 1 ? player2 : player1);
                return game;
            }
        }

        private sealed record PlayerData(string Kind, string Name, Symbol Symbol, int Score)
        {
            public static PlayerData FromPlayer(Player player)
            {
                return new PlayerData(player is Bot ? "Bot" : "Human", player.Name, player.Symbol, player.Score);
            }

            public Player ToPlayer()
            {
                Player player = Kind == "Bot" ? new Bot(Name, Symbol) : new Human(Name, Symbol);
                player.Score = Score;
                return player;
            }
        }

        private sealed record CubeData(Symbol Symbol, bool IsFrozen, int X, int Y);
    }
}