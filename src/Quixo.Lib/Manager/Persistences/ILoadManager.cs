using Quixo.Lib.Model;

namespace Quixo.Lib.Manager.Persistence
{
    public interface ILoadManager
    {
        /// <summary>
        /// Charge une partie sauvegardée à partir d'un fichier.
        /// </summary>
        Game? LoadGame(string Path);
    }
}