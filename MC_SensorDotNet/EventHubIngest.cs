using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_SensorDotNet
{
    public class EventHubIngest
    {
        public Task TelemetryIngest(Telemetry telemetry)
        {
            string serviceBusNamespace = "iotmc-ns";
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";
            string eventHubSASKey = "<< Add your SAS here >>";

            string eventHubConnectionString = ServiceBusConnectionStringBuilder.CreateUsingSharedAccessKey(
                ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, string.Empty),
                eventHubSASKeyName,
                eventHubSASKey);

            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);

            return Task.Run(async () =>
            {
                EventData eventData = new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(telemetry)));
                eventData.PartitionKey = telemetry.DeviceId.ToString(); 
                await eventHubClient.SendAsync(eventData);
            }); 
        }
    }
}
