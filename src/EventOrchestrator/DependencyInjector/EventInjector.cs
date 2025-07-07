namespace EasyEvent.DependencyInjector {
	using EasyEvent.Handler;
	using EasyEvent.Queue;
	using Microsoft.Extensions.DependencyInjection;
	public static class EventInjector {
		public static void AddEasyEvents(this IServiceCollection services) {
			services.AddSingleton<EventQueue>();
			services.AddHostedService<EventDispatcherWorker>();

			services.AddScoped<IEasyEvents, EazyEvents>();
		}
	}
}