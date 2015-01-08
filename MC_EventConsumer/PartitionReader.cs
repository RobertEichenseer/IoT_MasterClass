using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_EventConsumer
{
    public class PartitionReader
    {
        public async void ReadUsingEventHubProcessor()
        {
            string serviceBusNamespace = "iotmc-ns";
            string eventHubName = "iotmc";
            string eventHubSASKeyName = "Device01";
            string eventHubSASKey = "<< Add your SAS here >>";

            string storageAccountName = "iotmc";
            string storageAccountKey = "<< add your Storage Account key here >>"; 

            string storageConnectionString = String.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);
            string eventHubConnectionString = ServiceBusConnectionStringBuilder.CreateUsingSharedAccessKey(
                ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, string.Empty),
                eventHubSASKeyName,
                eventHubSASKey);

            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
            EventHubConsumerGroup eventHubConsumerGroup = eventHubClient.GetDefaultConsumerGroup();

            EventProcessorHost eventProcessorHost = new EventProcessorHost("MSTechDemoWorker", eventHubClient.Path, eventHubConsumerGroup.GroupName, eventHubConnectionString, storageConnectionString);
            await eventProcessorHost.RegisterEventProcessorAsync<EventHubEventProcessor>(); 
        }

        public void ReadFromEventHubPartition()
        {
            string serviceBusNamespace = "iotmc-ns";
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";
            int eventHubPartitionCount = 8;
            string eventHubSASKey = "<< Add your SAS here >>";

            string eventHubConnectionString = ServiceBusConnectionStringBuilder.CreateUsingSharedAccessKey(
                ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, string.Empty),
                eventHubSASKeyName,
                eventHubSASKey);

            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
            EventHubConsumerGroup eventHubConsumerGroup = eventHubClient.GetDefaultConsumerGroup();

            for (int i = 0; i < eventHubPartitionCount; i++)
            {
                string partitionId = i.ToString();
                Task.Run( async () =>
                {
                    //Just "new" message -> DateTime.Now
                    EventHubReceiver eventHubReceiver = eventHubConsumerGroup.CreateReceiver(partitionId, DateTime.Now);
                    //All existing messages in partition -> -1
                    //EventHubReceiver eventHubReceiver = await eventHubConsumerGroup.CreateReceiverAsync(partitionId, "-1");
                    do
                    {
                        EventData eventData = await eventHubReceiver.ReceiveAsync(TimeSpan.FromSeconds(2));
                        if (eventData != null)
                        {
                            string message = Encoding.UTF8.GetString(eventData.GetBytes());
                            string messageDetails = String.Format("Received: Seq number={0} Offset={1} Partition={2} EnqueueTimeUtc={3} Message={4}",
                                eventData.SequenceNumber,
                                eventData.Offset,
                                eventHubReceiver.PartitionId,
                                eventData.EnqueuedTimeUtc.ToShortTimeString(),
                                message);
                            Console.WriteLine(messageDetails);

                            //Store Partition-Id & Offset for further processing
                            string restorePoint_PartitionId = eventHubReceiver.PartitionId;
                            string restorePoint_Offset = eventData.Offset; 
                        }
                    } while (true);
                });
            }
        }
    }
}
