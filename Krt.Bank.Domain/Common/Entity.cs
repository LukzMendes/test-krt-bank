namespace Krt.Bank.Domain.Common
{
    public class Entity
    {
        public Id Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; }
        public DateTimeOffset? UpdatedAt { get; protected set; }
        public DateTimeOffset? RemovedAt { get; protected set; }
        public bool IsActive { get; protected set; }

        protected Entity()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            IsActive = true;
        }

        public void Remove()
        {
            RemovedAt = DateTimeOffset.UtcNow;
            IsActive = false;
        }

        public void Update()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Activate(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
