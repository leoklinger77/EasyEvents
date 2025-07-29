namespace EasyEvent {
	using EasyEvent.Events;

	/// <summary>
	/// Defines an abstraction for sending commands and publishing events through different execution models:
	/// synchronous, in-process, or asynchronous background processing.
	/// </summary>
	public interface IEasyEvents {
		/// <summary>
		/// Sends a command and awaits a response.
		/// Executed synchronously within the same context.
		/// </summary>
		/// <typeparam name="TCommand">The type of the command.</typeparam>
		/// <typeparam name="TResponse">The type of the response expected.</typeparam>
		/// <param name="command">The command instance to send.</param>
		/// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
		/// <returns>A task containing the response from the command handler.</returns>
		Task<TResponse> SendCommandAsync<TCommand, TResponse>(
			TCommand command,
			CancellationToken cancellationToken = default)
			where TCommand : ICommand;

		/// <summary>
		/// Publishes an event to be handled synchronously within the current request context.
		/// Used for in-process notifications that don't require a response.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="event">The event instance to publish.</param>
		/// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
		Task PublishEventAsync<TEvent>(
			TEvent @event,
			CancellationToken cancellationToken = default)
			where TEvent : IEvent;

		/// <summary>
		/// Publishes an event to a queue for asynchronous processing.
		/// Typically used for background tasks or integration with other systems.
		/// </summary>
		/// <typeparam name="TQueue">The type of the queued event.</typeparam>
		/// <param name="event">The event instance to queue.</param>
		/// <param name="cancellation">A token to observe while waiting for the task to complete.</param>
		Task PublishQueueAsync<TQueue>(
			TQueue @event,
			CancellationToken cancellation = default)
			where TQueue : IQueue;
	}
}