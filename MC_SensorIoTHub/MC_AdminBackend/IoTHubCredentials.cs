using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminBackend
{
    public static class IoTHubCredentials
    {
        //"Host name" in Azure portal
        public static string HostName = "[Your IoT Hub Name].azure-devices.net";

        //"Access policy name" in Azure portal
        public static string SharedAccessKeyName_IoTHubOwner = "iothubowner";
        //"Primary key" in Azure portal
        public static string SharedAccessKey_IoTHubOwner = "[Primary Key From Azure Portal]";

        //"Access policy name" in Azure portal
        public static string SharedAccesKeyName_Service = "service";
        //"Primary key" in Azure portal
        public static string SharedAdcessKey_Service = "[Primary Key from Azure Portal]";

        //Fixed Id given by Application
        public static string DeviceName = "[Your Device Id / Device Name]";
        //device.Authentication.SymmetricKey.PrimaryKey - After Device creation
        public static string DeviceIdentity = "[Your generated Device Primary Key]";
        
    }
}
