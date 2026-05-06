using CasinoApp.Player; 
using CasinoApp.Services;

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame : CasinoGameBase
    {
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;
        private List<Card> _playerCards;
        private List<Card> _dealerCards;

        private const decimal MaxBankValue = 3000;

        public BlackjackGame(PlayerProfile player, decimal bet)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Профиль игрока не может быть null.");
            if (bet <= 0)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка должна быть больше нуля.");
            if (bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка превышает баланс игрока.");

            _player = player;
            _bet = bet;

            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;
            this.OnDraw += UpdateProfileOnDraw;

            PlayGame();
        }

        protected override void FactoryMethod() { }

        public override void PlayGame()
        {
            Console.WriteLine("\n---=== Игра: Блэкджек ===---");

            _playerCards = new List<Card> { DrawCard(), DrawCard() };
            _dealerCards = new List<Card> { DrawCard(), DrawCard() };

            Console.WriteLine($"Ваши карты: {string.Join(", ", _playerCards)}");
            Console.WriteLine($"Карты дилера: {_dealerCards[0]}, *");

            bool isPlayerTurn = true;
            while (isPlayerTurn && CalculateScore(_playerCards) <= 21)
            {
                Console.WriteLine($"\nВаши очки: {CalculateScore(_playerCards)}");
                Console.WriteLine("1 - Взять карту");
                Console.WriteLine("2 - Остановиться");

                int choice = InputService.ReadInt("Ваш выбор: ", 1, 2);

                if (choice == 1)
                {
                    var card = DrawCard(); 
                    _playerCards.Add(card);
                    Console.WriteLine($"Вы взяли карту: {card}");

                    if (CalculateScore(_playerCards) > 21)
                    {
                        OnLooseInvoke("Перебор! У вас более 21 очка.");
                        isPlayerTurn = false;
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Вы решили остановиться.");
                    isPlayerTurn = false;
                }
            }

            int playerSum = CalculateScore(_playerCards);
            if (playerSum <= 21)
            {
                DealerTurn();
                DetermineWinner();
            }
        }


        #region Приватные методы механики

        private Card DrawCard()
        {
            return new Card(GetRandomSuit(), GetRandomRank());
        }

        private Suit GetRandomSuit() => (Suit)_random.Next(0, 4);
        private Rank GetRandomRank() => (Rank)_random.Next(6, 15);

        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(c => c.CardRank == Rank.Ace ? 11 : (int)c.CardRank);
            int aceCount = cards.Count(c => c.CardRank == Rank.Ace);

            while (score > 21 && aceCount > 0)
            {
                score -= 10; 
                aceCount--;
            }
            return score;
        }

        private void DealerTurn()
        {
            Console.WriteLine($"\nКарты дилера: {string.Join(", ", _dealerCards)}");
            while (CalculateScore(_dealerCards) < 17)
            {
                var card = DrawCard(); 
                _dealerCards.Add(card);
                Console.WriteLine($"Дилер взял карту: {card}");
            }
        }

        private void DetermineWinner()
        {
            int playerSum = CalculateScore(_playerCards);
            int dealerSum = CalculateScore(_dealerCards);

            Console.WriteLine($"\nИтог: У вас {playerSum}, у дилера {dealerSum}.");

            if (dealerSum > 21 || playerSum > dealerSum)
            {
                OnWinInvoke("Вы победили! Поздравляем!");
            }
            else if (playerSum == dealerSum)
            {
                OnDrawInvoke("Ничья!");
            }
            else
            {
                OnLooseInvoke("Вы проиграли.");
            }
        }

        #endregion

        #region Обработчики событий и сохранение

        private void UpdateProfileOnWin(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);

            decimal potentialNewBalance = _player.Balance + (_bet * 2);

            if (potentialNewBalance > MaxBankValue)
            {
                decimal overflowAmount = potentialNewBalance - MaxBankValue;
                _player.Balance = MaxBankValue;

                Console.WriteLine($"\n🎉🎉🎉 ВНИМАНИЕ! 🎉🎉🎉");
                Console.WriteLine($"Вы разорили казино! Ваш баланс ограничен: {MaxBankValue:N0}.");
                Console.WriteLine($"Излишек {overflowAmount:N2} пойдет на постройку нового заведения.");
                Console.WriteLine($"Ваш итоговый баланс: {_player.Balance:N0}");
            }
            else
            {
                _player.Balance += _bet * 2;
                Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
                _player.Wins++;
            }

            Console.ResetColor();
        }

        private void UpdateProfileOnLoose(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            _player.Balance -= _bet;
            Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
            _player.Losses++;
            Console.ResetColor();
        }

        private void UpdateProfileOnDraw(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.WriteLine($"Ваш баланс: {_player.Balance:N2}"); 
            Console.ResetColor();
        }

        #endregion
    }
}