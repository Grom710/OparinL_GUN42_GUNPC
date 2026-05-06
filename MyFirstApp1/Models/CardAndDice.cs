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

    public struct Card
    {
        public Suit CardSuit { get; }
        public Rank CardRank { get; }

        public Card(Suit suit, Rank rank)
        {
            CardSuit = suit;
            CardRank = rank;
        }

        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }
    }

    public class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException(string message) : base(message)
        {
        }
    }

    public struct Dice
    {
        private readonly Random _random = new Random(); 
        private int _number; 
        private readonly int _min;
        private readonly int _max;

        public int Number => _number;

        public Dice(int min, int max)
        {
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
            _number = 0; 

            Roll();
        }

        public void Roll()
        {

            _number = _random.Next(_min, _max + 1);
        }
    }
}