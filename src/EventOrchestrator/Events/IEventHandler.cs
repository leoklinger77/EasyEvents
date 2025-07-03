namespace EventOrchestrator.Events {
	public interface IEventHandler<TEvent> where TEvent : IEvent {
		Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
	}
}
