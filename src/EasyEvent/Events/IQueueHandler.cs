namespace EasyEvent.Events {
	public interface IQueueHandler<TEvent> where TEvent : IQueue {
		Task HandleAsync(TEvent @event, IServiceProvider scope, CancellationToken cancellationToken = default);
	}
}