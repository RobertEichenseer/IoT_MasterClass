using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC_SensorDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            bool useFieldGateway = false; 

            EventHubIngest eventHubIngest = new EventHubIngest();
            FieldGatewayIngest fieldGatewayIngest = new FieldGatewayIngest();

            Random random = new Random(DateTime.Now.Millisecond);

            List<Task> ingests = new List<Task>();
            for (int x = 0; x < 100; x++ )
            { 
                for (int i = 0; i < 10; i++)
                {
                    Telemetry telemetry = new Telemetry();
                    telemetry.DeviceId = "Device-50";
                    telemetry.Temperature = 27.5;
                    telemetry.Humidity = 68.3;
                    telemetry.Pollution = random.NextDouble() * 100;
                    
                    if (useFieldGateway)
                        ingests.Add(fieldGatewayIngest.TelemetryIngest(telemetry));
                    else
                        ingests.Add(eventHubIngest.TelemetryIngest(telemetry));
                }
                Thread.Sleep(500); 
            }
            Task.WaitAll(ingests.ToArray<Task>()); 
        }
    }
}
