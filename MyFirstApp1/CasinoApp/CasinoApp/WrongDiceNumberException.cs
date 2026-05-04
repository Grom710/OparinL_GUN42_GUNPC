
namespace CasinoApp.CasinoApp
{
    [Serializable]
    internal class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException()
        {
        }

        public WrongDiceNumberException(string? message) : base(message)
        {
        }

        public WrongDiceNumberException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}