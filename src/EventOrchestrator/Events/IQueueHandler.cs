namespace EventOrchestrator.Events {
	using Microsoft.Extensions.DependencyInjection;
	public interface IQueueHandler<TEvent> where TEvent : IQueue {
		Task HandleAsync(TEvent @event, IServiceScope scope, CancellationToken cancellationToken = default);
	}
}