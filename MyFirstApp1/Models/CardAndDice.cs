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
        private readonly Random _random = new Random(); // Генератор для каждого кубика
        private int _number; // Убираем readonly, чтобы можно было перекидывать
        private readonly int _min;
        private readonly int _max;

        // Свойство Number остается как есть, оно просто возвращает значение
        public int Number => _number;

        public Dice(int min, int max)
        {
            // Проверки параметров (как и было)
            if (min < 1 || min > int.MaxValue || max < 1 || max > int.MaxValue)
                throw new WrongDiceNumberException(
                    $"Неверный диапазон. Задано: min={min}, max={max}. Допустимый диапазон: от 1 до {int.MaxValue}."
                );
            if (min > max)
                throw new WrongDiceNumberException(
                    $"Неверный диапазон. Минимальное значение ({min}) не может быть больше максимального ({max})."
                );

            _min = min;
            _max = max;
            _number = 0; // Начальное значение

            // Бросаем кубик сразу при создании
            Roll();
        }

        // НОВЫЙ МЕТОД: Перекидывает кубик
        public void Roll()
        {
            // Генерируем новое случайное число в заданном диапазоне
            // +1 потому что верхняя граница в Next() не включается
            _number = _random.Next(_min, _max + 1);
        }
    }
}