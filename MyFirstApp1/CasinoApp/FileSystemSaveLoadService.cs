// CasinoApp/Services/FileSystemSaveLoadService.cs
using System;
using System.IO;
using CasinoApp.Services;

namespace CasinoApp.Services
{
    /// <summary>
    /// Универсальный сервис для сохранения и загрузки строковых данных в файловую систему.
    /// </summary>
    public class FileSystemSaveLoadService : ISaveLoadService<string>
    {
        // Поле для хранения базового пути, переданного в конструкторе
        private readonly string _basePath;

        /// <summary>
        /// Конструктор, принимающий путь к папке для хранения файлов.
        /// </summary>
        /// <param name="basePath">Путь к директории (например, "data/files" или "C:/Saves")</param>
        public FileSystemSaveLoadService(string basePath)
        {
            _basePath = basePath;
            // Создаем директорию сразу при создании объекта, чтобы быть уверенными в ее наличии
            Directory.CreateDirectory(_basePath);
        }

        /// <summary>
        /// Сохраняет строковые данные в файл.
        /// </summary>
        /// <param name="data">Строка, которую нужно сохранить.</param>
        /// <param name="id">Идентификатор, который станет именем файла.</param>
        public void SaveData(string data, string id)
        {
            // Формируем полный путь к файлу. Добавляем расширение .txt.
            // Path.Combine корректно обрабатывает слэши в разных ОС.
            string filePath = Path.Combine(_basePath, id + ".txt");

            // Записываем строку в файл. Если файл существует, он будет перезаписан.
            File.WriteAllText(filePath, data);

            Console.WriteLine($"[FileSystemService] Данные сохранены в файл: {filePath}");
        }

        /// <summary>
        /// Загружает строковые данные из файла.
        /// </summary>
        /// <param name="id">Идентификатор (имя файла) для загрузки.</param>
        /// <returns>Содержимое файла или null, если файл не найден.</returns>
        public string LoadData(string id)
        {
            string filePath = Path.Combine(_basePath, id + ".txt");

            // Проверяем, существует ли файл
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[FileSystemService] Файл не найден: {filePath}");
                return null; // Или return default(string);
            }

            try
            {
                // Читаем всё содержимое файла и возвращаем его как одну строку.
                string loadedData = File.ReadAllText(filePath);
                Console.WriteLine($"[FileSystemService] Данные загружены из файла: {filePath}");
                return loadedData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return null;
            }
        }
    }
}