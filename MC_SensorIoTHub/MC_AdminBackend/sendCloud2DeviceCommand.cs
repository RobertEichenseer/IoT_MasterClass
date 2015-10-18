using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminBackend
{
    public class SendCloud2DeviceCommand
    {
        private string _connectionString;

        public SendCloud2DeviceCommand()
        {
            _connectionString = "HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}";
            _connectionString = String.Format(_connectionString, 
                IoTHubCredentials.HostName, 
                IoTHubCredentials.SharedAccesKeyName_Service, 
                IoTHubCredentials.SharedAdcessKey_Service);

        }

        internal void SendCommand()
        {
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
            
            Message message = new Message(Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString()));
            message.ExpiryTimeUtc = DateTime.Now.ToUniversalTime().AddMinutes(1);
            message.Ack = DeliveryAcknowledgement.Full;

            serviceClient.SendAsync(IoTHubCredentials.DeviceName, message).Wait();

        }
    }
}
