// CasinoApp/CardAndDice.cs

using System;

namespace CasinoApp
{
    // 1. Перечисление мастей карт
    public enum Suit
    {
        Hearts,   // Черви
        Diamonds, // Бубны
        Clubs,    // Трефы
        Spades    // Пики
    }

    // 2. Перечисление величин карт (от 6 до Туза)
    public enum Rank
    {
        Six = 6,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,  // Валет
        Queen, // Дама
        King,  // Король
        Ace    // Туз
    }

    // 3. Структура Card (Карта)
    public struct Card
    {
        // Readonly свойства: их можно установить только в конструкторе
        public Suit CardSuit { get; }
        public Rank CardRank { get; }

        public Card(Suit suit, Rank rank)
        {
            CardSuit = suit;
            CardRank = rank;
        }

        // Переопределим ToString() для красивого вывода карты в консоль
        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }
    }

    // 4. Собственное исключение для кубика
    public class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException(string message) : base(message)
        {
            // Конструктор просто передает сообщение в базовый класс Exception
        }
    }

    // 5. Структура Dice (Кубик)
    public struct Dice
    {
        private readonly int _min;
        private readonly int _max;

        // Readonly свойство, которое возвращает результат броска
        public int Number { get; }

        private static readonly Random _random = new Random();

        public Dice(int min, int max)
        {
            // 5.1. Проверка корректности чисел в конструкторе
            if (min < 1 || min > int.MaxValue || max < 1 || max > int.MaxValue)
            {
                // 5.2. Выбрасываем наше кастомное исключение с понятным сообщением
                throw new WrongDiceNumberException(
                    $"Неверный диапазон. Задано: min={min}, max={max}. " +
                    $"Допустимый диапазон для обоих чисел: от 1 до {int.MaxValue}."
                );
            }

            if (min > max)
            {
                throw new WrongDiceNumberException(
                    $"Неверный диапазон. Минимальное значение ({min}) не может быть больше максимального ({max})."
                );
            }

            _min = min;
            _max = max;

            // При создании кубика сразу бросаем его (генерируем число)
            Number = _random.Next(_min, _max + 1); // +1, так как верхняя граница в Random не включается
        }
    }
}