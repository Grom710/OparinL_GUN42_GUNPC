// CasinoApp/Games/CasinoGameBase.cs

using System;

namespace CasinoApp.Games
{
    // 1. Создаем делегат для событий. Он определяет сигнатуру методов, которые будут вызываться.
    public delegate void GameResultHandler(string message);

    /// <summary>
    /// Абстрактный базовый класс для всех игр казино.
    /// Реализует общую логику событий и вывода результатов.
    /// </summary>
    public abstract class CasinoGameBase
    {
        // 2. Объявляем события. Они публичные, чтобы на них могли подписываться извне.
        public event GameResultHandler OnWin;
        public event GameResultHandler OnLoose;
        public event GameResultHandler OnDraw;

        /// <summary>
        /// Конструктор базового класса. Вызывает фабричный метод.
        /// </summary>
        protected CasinoGameBase()
        {
            // Вызов абстрактного фабричного метода сразу после создания объекта.
            // Здесь можно было бы проверять результат FactoryMethod на null.
            FactoryMethod();
        }

        // 3. Абстрактный метод, который должны реализовать все наследники.
        // Это и есть "точка входа" для запуска механики игры.
        public abstract void PlayGame();

        // 4. Абстрактный фабричный метод.
        // Он нужен, если в будущем базовый класс будет создавать какие-то общие ресурсы.
        // Сейчас он пуст, но по ТЗ он должен быть.
        protected abstract void FactoryMethod();

        // 5. Защищенные методы для вызова событий.
        // Они проверяют, есть ли подписчики, прежде чем вызывать событие (защита от NullReference).

        protected void OnWinInvoke(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[ИГРА] ВЫИГРЫШ: {message}");
            Console.ResetColor();
            OnWin?.Invoke(message);
        }

        protected void OnLooseInvoke(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ИГРА] ПРОИГРЫШ: {message}");
            Console.ResetColor();
            OnLoose?.Invoke(message);
        }

        protected void OnDrawInvoke(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ИГРА] НИЧЬЯ: {message}");
            Console.ResetColor();
            OnDraw?.Invoke(message);
        }
    }
}