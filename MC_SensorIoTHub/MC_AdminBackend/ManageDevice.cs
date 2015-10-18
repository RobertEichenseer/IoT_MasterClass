using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminBackend
{
    public class ManageDevice
    {
        private string _connectionString; 
        
        public ManageDevice()
        {
            _connectionString = "HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}";
            _connectionString = String.Format(_connectionString, 
                                        IoTHubCredentials.HostName,
                                        IoTHubCredentials.SharedAccessKeyName_IoTHubOwner,
                                        IoTHubCredentials.SharedAccessKey_IoTHubOwner
                                        );
        }

        public async Task<Device> AddRetrieveDevice()
        {
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
            
            Device device;
            device = await registryManager.GetDeviceAsync(IoTHubCredentials.DeviceName); 
            if (device == null)
               device = await registryManager.AddDeviceAsync(new Device(IoTHubCredentials.DeviceName));
            
            return device; 
        }
        
    }
}
