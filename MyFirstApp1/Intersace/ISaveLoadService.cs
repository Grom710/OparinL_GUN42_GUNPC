

namespace CasinoApp.Services
{

    /// <typeparam name="T">Тип данных, с которым работает сервис.</typeparam>
    public interface ISaveLoadService<T>
    {

        /// <param name="data">Данные для сохранения.</param>
        /// <param name="id">Уникальный строковый идентификатор (например, имя файла).</param>
        void SaveData(T data, string id);


        /// <param name="id">Уникальный строковый идентификатор.</param>
        /// <returns>Загруженные данные или значение по умолчанию, если данных нет.</returns>
        T LoadData(string id);
    }
}