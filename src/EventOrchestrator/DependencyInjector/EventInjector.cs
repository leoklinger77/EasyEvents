namespace EventOrchestrator.DependencyInjector {
	using EventOrchestrator.Handler;
	using EventOrchestrator.Queue;
	using Microsoft.Extensions.DependencyInjection;

	public static class EventInjector {
		public static void AddEazyEvents(this IServiceCollection services) {
			services.AddSingleton<EventQueue>();
			services.AddHostedService<EventDispatcherWorker>();

			services.AddScoped<IEazyEvents, EazyEvents>();
		}
	}
}