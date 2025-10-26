using Krt.Bank.Domain.Exceptions;

namespace Krt.Bank.Domain.Common
{
    public abstract class Id
    {
        public Guid Value { get; private set; }

        protected Id(Guid value)
        {
            if (value == Guid.Empty)
                throw new DomainException("Id não pode ser vazio");

            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override bool Equals(object? obj) => obj is Id other && Value.Equals(other.Value);
        public static bool operator ==(Id? a, Id? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Id? a, Id? b) => !(a == b);
    }
}
