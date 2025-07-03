namespace EventOrchestrator.Queue {
	using EventOrchestrator.Events;
	using System.Threading.Channels;
	public class EventQueue {
		private readonly Channel<IQueue> _channel = Channel.CreateUnbounded<IQueue>();
		public async Task EnqueueAsync(IQueue @event) {
			await _channel.Writer.WriteAsync(@event);
		}
		public ChannelReader<IQueue> Reader => _channel.Reader;
	}
}