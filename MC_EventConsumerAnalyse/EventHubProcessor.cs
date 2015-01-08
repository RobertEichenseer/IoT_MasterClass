using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MC_EventConsumerAnalyse
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

                Telemetry telemetry = JsonConvert.DeserializeObject<Telemetry>(Encoding.UTF8.GetString(eventData.GetBytes()));
                AnalyticsEngine._humidityTelemetryIngest.Push(telemetry.Humidity);
                AnalyticsEngine._pollutionTelemetryIngest.Push(telemetry.Pollution); 

                Debug.WriteLine(telemetry.Pollution.ToString(), eventData.EnqueuedTimeUtc.ToString()); 

            }
        }
    }
}

