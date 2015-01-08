using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_EventConsumer
{

    public class EventHubEventProcessor : IEventProcessor
    {
        PartitionContext _partitionContext;

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown)
                await _partitionContext.CheckpointAsync();
        }

        public Task OpenAsync(PartitionContext context)
        {
            _partitionContext = context;
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string messageDetails = String.Format("Received: Seq number={0} Offset={1} Partition={2} EnqueueTimeUtc={3} Message={4}",
                                eventData.SequenceNumber,
                                eventData.Offset,
                                eventData.PartitionKey,
                                eventData.EnqueuedTimeUtc.ToShortTimeString(),
                                Encoding.UTF8.GetString(eventData.GetBytes()));
            }
        }
    }
}

