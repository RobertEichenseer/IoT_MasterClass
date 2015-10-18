using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminBackend
{

    public class GetDeliveryStatus
    {
        
        private string _connectionString;
        public GetDeliveryStatus()
        {
            _connectionString = "HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}";
            _connectionString = String.Format(_connectionString,
                IoTHubCredentials.HostName,
                IoTHubCredentials.SharedAccesKeyName_Service,
                IoTHubCredentials.SharedAdcessKey_Service);
        }

        internal async void GetStatus()
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            while (true)
            {
                Console.WriteLine("Requesting Delivery Status: {0}", DateTime.Now.ToUniversalTime());

                FeedbackBatch feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null)
                    continue;

                Console.WriteLine("Received feedback: {0}", string.Join(" - ", feedbackBatch.Records.Select(deliveryState => deliveryState.StatusCode)));
                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }

    }
}
