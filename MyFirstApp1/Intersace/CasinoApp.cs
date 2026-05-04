// Создаем новый файл для интерфейса
namespace CasinoApp
{
    /// <summary>
    /// Интерфейс для управления игровым процессом.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Запускает игровой процесс.
        /// </summary>
        void StartGame();
    }
}