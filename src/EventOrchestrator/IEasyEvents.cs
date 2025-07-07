namespace EasyEvent {
	using EasyEvent.Events;
	public interface IEasyEvents {
		Task<TResponse> SendCommandAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
			where TCommand : IEvent;

		Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
			where TEvent : IEvent;

		Task PublishQueueAsync<TQueue>(TQueue @event, CancellationToken cancellation = default)
			where TQueue : IQueue;
	}
}