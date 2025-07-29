namespace EasyEvent.Queue {
    using EasyEvent.Events;
    using System.Threading.Channels;
    public class EventQueue {
        private readonly Channel<IQueue> _channel;
        public EventQueue(UnboundedChannelOptions? unboundedOptions = null) {
            _channel = Channel.CreateUnbounded<IQueue>(unboundedOptions ?? new UnboundedChannelOptions());
        }

        public EventQueue(BoundedChannelOptions boundedOptions) {
            _channel = Channel.CreateBounded<IQueue>(boundedOptions ?? throw new ArgumentNullException(nameof(boundedOptions)));
        }

        public async Task EnqueueAsync(IQueue @event) {
            await _channel.Writer.WriteAsync(@event);
        }

        public ChannelReader<IQueue> Reader => _channel.Reader;
    }
}