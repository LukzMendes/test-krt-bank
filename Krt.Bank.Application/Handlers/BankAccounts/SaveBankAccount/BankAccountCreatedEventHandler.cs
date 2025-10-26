using Krt.Bank.Application.Handlers.BankAccounts.Dtos;
using Krt.Bank.Application.Interfaces.Events;
using Krt.Bank.Events.BankAccounts;
using Microsoft.Extensions.Logging;

namespace Krt.Bank.Application.Handlers.BankAccounts.SaveBankAccount
{
    public class BankAccountCreatedEventHandler
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<BankAccountCreatedEventHandler> _logger;

        public BankAccountCreatedEventHandler(
            IEventPublisher eventPublisher,
            ILogger<BankAccountCreatedEventHandler> logger)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task Handle(BankAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id.HasValue)
                {
                    var updatedEvent = new BankAccountUpdatedEvent(
                        bankAccountId: request.Id.Value,
                        userId: request.UserId,
                        newAccountNumber: request.AccountNumber!,
                        updatedAt: DateTime.UtcNow);

                    await _eventPublisher.PublishAsync(updatedEvent, cancellationToken);

                    _logger.LogInformation("Evento de atualização de conta publicado: {BankAccountId}", updatedEvent.BankAccountId);
                }
                else
                {
                    var createdEvent = new BankAccountCreatedEvent(
                        bankAccountId: Guid.NewGuid(), 
                        userId: request.UserId,
                        accountNumber: request.AccountNumber!,
                        createdAt: DateTime.UtcNow);

                    await _eventPublisher.PublishAsync(createdEvent, cancellationToken);

                    _logger.LogInformation("Evento de criação de conta publicado: {BankAccountId}", createdEvent.BankAccountId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar evento de conta bancária.");
                throw;
            }
        }
    }
}
