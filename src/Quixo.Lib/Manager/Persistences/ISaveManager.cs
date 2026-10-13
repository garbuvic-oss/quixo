using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Persistence
{
    public interface ISaveManager
    {
        /// <summary>
        /// Sauvegarde la partie en cours dans un fichier à l'emplacement spécifié.
        /// </summary>
        bool SaveGame(Game game, string Path);
    }
}