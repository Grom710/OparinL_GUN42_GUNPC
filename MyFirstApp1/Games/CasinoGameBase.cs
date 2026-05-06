using System;

namespace CasinoApp.Games
{
    public delegate void GameResultHandler(string message);


    public abstract class CasinoGameBase
    {
        public event GameResultHandler OnWin;
        public event GameResultHandler OnLoose;
        public event GameResultHandler OnDraw;

        protected CasinoGameBase()
        {

            FactoryMethod();
        }

        public abstract void PlayGame();

        protected abstract void FactoryMethod();


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