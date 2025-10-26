using Krt.Bank.Domain.Exceptions;

namespace Krt.Bank.Domain.Common
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public static Money Zero => new Money(0, "BRL");

        public Money()
        {
        }

        private Money(decimal amount, string currency, bool round = true)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Moeda não pode ser nula ou vazia");

            Amount = round ? Math.Round(amount, 5, MidpointRounding.AwayFromZero) : amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string? currency = "BRL")
        {
            return new Money(amount, currency);
        }

        //Aqui devemos adicionar outras operações e validações como subtração, multiplicação, não ser possívei dividir por zero etc.
    }

    public static class Currency
    {
        public static string BRL = "BRL";
    }
}
