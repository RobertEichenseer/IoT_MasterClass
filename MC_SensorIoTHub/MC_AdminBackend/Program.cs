using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;

namespace MC_AdminBackend
{
    class Program
    {
        
        static void Main(string[] args)
        {

            //Create-Retrieve Device
            ManageDevice manageDevice = new ManageDevice();
            Task<Device> taskDevice = manageDevice.AddRetrieveDevice();
            Device device = taskDevice.Result;
            IoTHubCredentials.DeviceIdentity = device.Authentication.SymmetricKey.PrimaryKey;
            Console.WriteLine("Device: {0}", device.Authentication.SymmetricKey.PrimaryKey);

            //Send Cloud 2 Device Command
            SendCloud2DeviceCommand sendCloud2DeviceCommand = new SendCloud2DeviceCommand();
            sendCloud2DeviceCommand.SendCommand();

            //Get Delivery Status
            GetDeliveryStatus getDeliveryStatus = new GetDeliveryStatus();
            getDeliveryStatus.GetStatus();
            

        }
    }
}
