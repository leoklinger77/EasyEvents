namespace EventOrchestrator.Handler {
	using EventOrchestrator.Events;
	using EventOrchestrator.Queue;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public class EazyEvents : IEazyEvents {
		private readonly ILogger<EazyEvents> _logger;
		private readonly IServiceProvider _provider;
		private readonly EventQueue _eventQueue;

		public EazyEvents(ILogger<EazyEvents> logger,
						  IServiceProvider provider,
						  EventQueue eventQueue) {
			_eventQueue = eventQueue;
			_provider = provider;
			_logger = logger;
		}

		public async Task<TResponse> SendCommandAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
			where TCommand : IEvent {
			if (_logger.IsEnabled(LogLevel.Trace)) {
				_logger.LogTrace($"[EazyEvents] Send command initialize {command.GetType().Name}");
			}
			var handler = _provider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
			var handlerResult = await handler.HandleAsync(command, cancellationToken);

			if (_logger.IsEnabled(LogLevel.Trace)) {
				_logger.LogTrace($"[EazyEvents] Send command finish {command.GetType().Name}");
			}
			return handlerResult;
		}

		public async Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
			where TEvent : IEvent {
			if (_logger.IsEnabled(LogLevel.Trace)) {
				_logger.LogTrace($"[EazyEvents] Publish event initialize {@event.GetType().Name}");
			}

			var handlers = _provider.GetServices<IEventHandler<TEvent>>();
			foreach (var handler in handlers) {
				await handler.HandleAsync(@event, cancellationToken);
				if (_logger.IsEnabled(LogLevel.Trace)) {
					_logger.LogTrace($"[EazyEvents] Publish event finish {@event.GetType().Name}");
				}
			}
		}

		public async Task PublishQueueAsync<TQueue>(TQueue @event, CancellationToken cancellation = default)
			where TQueue : IQueue {
			if (_logger.IsEnabled(LogLevel.Trace)) {
				_logger.LogTrace($"[EazyEvents] Publish queue initialize {@event.GetType().Name}");
			}

			await _eventQueue.EnqueueAsync(@event);

			if (_logger.IsEnabled(LogLevel.Trace)) {
				_logger.LogTrace($"[EazyEvents] Publish queue finish {@event.GetType().Name}");
			}
		}
	}
}