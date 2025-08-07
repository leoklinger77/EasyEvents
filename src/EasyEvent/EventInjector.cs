namespace EasyEvent {
    using EasyEvent.Handler;
    using EasyEvent.Options;
    using EasyEvent.Queue;
    using Microsoft.Extensions.DependencyInjection;
    public static class EventInjector {
        public static void AddEasyEvents(this IServiceCollection services, Action<EasyOptions>? configure = null) {
            var options = new EasyOptions();
            configure?.Invoke(options);

            var hasBounded = options.QueueBounded is not null;
            var hasUnbounded = options.QueueUnbounded is not null;

            if (hasBounded && hasUnbounded) {
                throw new InvalidOperationException("Only one of 'QueueBounded' or 'QueueUnbounded' should be configured, not both.");
            }

            EventQueue queue;
            if (options.QueueBounded is not null) {
                queue = new EventQueue(options.QueueBounded);
            } else if (options.QueueUnbounded is not null) {
                queue = new EventQueue(options.QueueUnbounded);
            } else {
                queue = new EventQueue();
            }
            services.AddSingleton(queue);

            services.AddHostedService<EventDispatcherWorker>();

            services.AddScoped<IEasyEvents, EazyEvents>();
        }
    }
}