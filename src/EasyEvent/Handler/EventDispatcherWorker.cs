namespace EasyEvent.Handler {
	using EasyEvent.Events;
	using EasyEvent.Queue;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	public class EventDispatcherWorker : BackgroundService {
		private readonly ILogger<EventDispatcherWorker> _logger;
		private readonly IServiceProvider _provider;
		private readonly EventQueue _queue;

		public EventDispatcherWorker(ILogger<EventDispatcherWorker> logger, IServiceProvider provider, EventQueue queue) {
			_logger = logger;
			_provider = provider;
			_queue = queue;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			await foreach (var @event in _queue.Reader.ReadAllAsync(stoppingToken)) {
				var getType = @event.GetType();
				if (_logger.IsEnabled(LogLevel.Trace)) {
					_logger.LogTrace($"Receiving event '{getType.Name}' in processing queue");
				}
				using IServiceScope scope = _provider.CreateScope();
				try {
					var serviceProvider = scope.ServiceProvider;

					var handlerType = typeof(IQueueHandler<>).MakeGenericType(getType);
					var handlers = serviceProvider.GetServices(handlerType);

					foreach (var handler in handlers) {
						var method = handlerType.GetMethod(nameof(IQueueHandler<IQueue>.HandleAsync))!;
						var task = (Task)method.Invoke(handler, [@event, scope.ServiceProvider, stoppingToken])!;
						await task;
					}
				} catch (Exception ex) {
					_logger.LogError($"Error processing event {getType.Name}: {ex}");
				} finally {
					scope?.Dispose();
					if (_logger.IsEnabled(LogLevel.Trace)) {
						_logger.LogTrace($"Event processing completed '{getType.Name}' in processing queue");
					}
				}
			}
		}
	}
}
