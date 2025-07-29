using System.Threading.Channels;

namespace EasyEvent.Options {
    public class EasyOptions {
        public UnboundedChannelOptions? QueueUnbounded { get; set; }
        public BoundedChannelOptions? QueueBounded { get; set; }
    }
}
