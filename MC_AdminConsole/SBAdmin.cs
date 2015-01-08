using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminConsole
{
    public class SBAdmin
    {
        public async Task<EventHubDescription> CreateEventHubByConnectionStringAsync()
        {
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";
            string serviceBusConnectionString = "Endpoint=sb://iotmc-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<<Add your SAS here>>"; 

            NamespaceManager nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            string eventHubKey = SharedAccessAuthorizationRule.GenerateRandomKey();
            EventHubDescription eventHubDescription = new EventHubDescription(eventHubName)
            {
                PartitionCount = 8,
                MessageRetentionInDays = 1
            };
            SharedAccessAuthorizationRule eventHubSendRule = new SharedAccessAuthorizationRule(eventHubSASKeyName, eventHubKey, 
                new[] { AccessRights.Send, AccessRights.Listen });
            eventHubDescription.Authorization.Add(eventHubSendRule);
            eventHubDescription = await nameSpaceManager.CreateEventHubIfNotExistsAsync(eventHubDescription);

            string eventHubSASKey = ((SharedAccessAuthorizationRule)eventHubDescription.Authorization.First()).PrimaryKey;

            return eventHubDescription; 
        }

        public async Task<EventHubDescription> CreateEventHubAsync()
        {

            string serviceBusNamespace = "iotmc-ns";
            string serviceBusManageKey = "<< Add your SAS here >>";
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";

            Uri serviceBusUri = ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, string.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", serviceBusManageKey);
            NamespaceManager nameSpaceManager = new NamespaceManager(string.Format("//{0}/", serviceBusUri.Host), tokenProvider);
            string eventHubKey = SharedAccessAuthorizationRule.GenerateRandomKey();

            EventHubDescription eventHubDescription = new EventHubDescription(eventHubName)
            {
                PartitionCount = 8,
                MessageRetentionInDays = 1
            };
            SharedAccessAuthorizationRule eventHubSendRule = new SharedAccessAuthorizationRule(eventHubSASKeyName, eventHubKey, new[] { AccessRights.Send, AccessRights.Listen });
            eventHubDescription.Authorization.Add(eventHubSendRule);
            eventHubDescription = await nameSpaceManager.CreateEventHubIfNotExistsAsync(eventHubDescription);

            string eventHubSASKey = ((SharedAccessAuthorizationRule)eventHubDescription.Authorization.First()).PrimaryKey;

            return eventHubDescription;
        }
    }
}
