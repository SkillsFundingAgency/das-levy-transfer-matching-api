namespace SFA.DAS.LevyTransferMatching.Abstractions.Events;

public interface IDomainEventDispatcher
{
    Task Send<TDomainEvent>(TDomainEvent @event, CancellationToken cancellationToken = default) where TDomainEvent : IDomainEvent;
}