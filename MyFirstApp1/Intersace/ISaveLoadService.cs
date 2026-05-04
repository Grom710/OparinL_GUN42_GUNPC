// CasinoApp/Services/ISaveLoadService.cs

namespace CasinoApp.Services
{
    /// <summary>
    /// Универсальный интерфейс для сохранения и загрузки данных.
    /// </summary>
    /// <typeparam name="T">Тип данных, с которым работает сервис.</typeparam>
    public interface ISaveLoadService<T>
    {
        /// <summary>
        /// Сохраняет данные типа T по указанному идентификатору.
        /// </summary>
        /// <param name="data">Данные для сохранения.</param>
        /// <param name="id">Уникальный строковый идентификатор (например, имя файла).</param>
        void SaveData(T data, string id);

        /// <summary>
        /// Загружает данные типа T по указанному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный строковый идентификатор.</param>
        /// <returns>Загруженные данные или значение по умолчанию, если данных нет.</returns>
        T LoadData(string id);
    }
}