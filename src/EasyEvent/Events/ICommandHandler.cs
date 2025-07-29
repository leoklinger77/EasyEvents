namespace EasyEvent.Events {
	public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand {
		Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
	}
}