namespace EventOrchestrator.Events {
	public interface ICommandHandler<TCommand, TResponse> where TCommand : IEvent {
		Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
	}
}