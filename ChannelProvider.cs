using Confluent.Kafka;
using System.Threading.Channels;

namespace KafkaParallelConsumer;

/// <summary>
/// Channel Procider to connect topic partition
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public sealed class ChannelProvider<TKey, TValue>
{
    private readonly Dictionary<TopicPartition, Channel<ConsumeResult<TKey, TValue>>> channels;
    private readonly Dictionary<TopicPartition, Task> workers;

    public ChannelProvider()
    {
        this.channels = new Dictionary<TopicPartition, Channel<ConsumeResult<TKey, TValue>>>();
        this.workers = new Dictionary<TopicPartition, Task>();
    }

    /// <summary>
    /// Create topic partition channel 
    /// </summary>
    /// <param name="topicPartition"></param>

    public void CreateTopicPartitionChannel(TopicPartition topicPartition)
    {
        if (!channels.ContainsKey(topicPartition))
        {
            var channel = Channel.CreateUnbounded<ConsumeResult<TKey, TValue>>(new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = true,
            });

            channels.Add(topicPartition, channel);
        }
    }

    /// <summary>
    /// Write channel 
    /// </summary>
    /// <param name="consumer"></param>
    /// <param name="topicPartition"></param>
    /// <param name="processingAction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public ChannelWriter<ConsumeResult<TKey, TValue>> GetChannelWriter(IConsumer<TKey, TValue> consumer, TopicPartition topicPartition,
        Func<ConsumeResult<TKey, TValue>, CancellationToken, Task> processingAction, CancellationToken cancellationToken)
    {
        var channel = channels[topicPartition];

        if (!workers.ContainsKey(topicPartition))
        {
            var topicWorker = new TopicPartitionWorker<TKey, TValue>(consumer, channel.Reader, processingAction, commitSynchronous: false);

            workers.Add(topicPartition, topicWorker.ExecuteAsync(cancellationToken));
        }

        return channel.Writer;
    }

    public void CompleteTopicPartitionChannel(TopicPartition topicPartition)
    {
        var channel = channels[topicPartition];
        channel.Writer.Complete();
    }
}
