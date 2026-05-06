
using System;
using System.IO;
using CasinoApp.Services;

namespace CasinoApp.Services
{

    public class FileSystemSaveLoadService : ISaveLoadService<string>
    {
        private readonly string _basePath;

        /// <param name="basePath">Путь к директории (например, "data/files" или "C:/Saves")</param>
        public FileSystemSaveLoadService(string basePath)
        {
            _basePath = basePath;
            Directory.CreateDirectory(_basePath);
        }

        /// <param name="data">Строка, которую нужно сохранить.</param>
        /// <param name="id">Идентификатор, который станет именем файла.</param>
        public void SaveData(string data, string id)
        {
            string filePath = Path.Combine(_basePath, id + ".txt");

            File.WriteAllText(filePath, data);

            Console.WriteLine($"[FileSystemService] Данные сохранены в файл: {filePath}");
        }

        /// <param name="id">Идентификатор (имя файла) для загрузки.</param>
        /// <returns>Содержимое файла или null, если файл не найден.</returns>
        public string LoadData(string id)
        {
            string filePath = Path.Combine(_basePath, id + ".txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[FileSystemService] Файл не найден: {filePath}");
                return null; 
            }

            try
            {
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