using Krt.Bank.Domain.Common;

namespace Krt.Bank.Domain.Users
{

    public class User : Entity
    {
        public UserId Id { get; private set; }
        public string Name { get; private set; }
        public string CPF { get; private set; }

        private User() { }

        private User(UserId id, string name, string cpf)
        {
            Id = id;
            Name = name;

            if (!IsValidCPF(cpf))
                throw new ArgumentException("O CPF informado é inválido.", nameof(cpf));
            CPF = cpf;
        }

        public static User Create(string name, string cpf)
        {
            return new User(UserId.Create(Guid.NewGuid()), name, cpf);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("O novo nome não pode estar vazio.", nameof(newName));

            Name = newName;
            Update();
        }

        public void UpdateCPF(string newCpf)
        {
            if (string.IsNullOrWhiteSpace(newCpf))
                throw new ArgumentException("O novo CPF não pode estar vazio.", nameof(newCpf));
            if (!IsValidCPF(newCpf))
                throw new ArgumentException("O CPF informado é inválido.", nameof(newCpf));

            CPF = newCpf;
            Update();
        }

        public void ChangeStatus(bool isActive)
        {
            Activate(isActive);
        }

        private bool IsValidCPF(string cpf)
        {
            return cpf != null && cpf.All(char.IsDigit) && cpf.Length == 11;
        }


        public class UserId : Id
        {
            public UserId(Guid value) : base(value)
            {
            }

            public static UserId Create(Guid value)
            {
                return new UserId(value);
            }
        }
    }
}
