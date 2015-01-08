using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_AdminConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            SBAdmin sbAdmin = new SBAdmin();

            Task<EventHubDescription> eventHubCreation = sbAdmin.CreateEventHubAsync();
            eventHubCreation.Wait();
            EventHubDescription eventHubDescription = eventHubCreation.Result;
            Console.WriteLine(eventHubDescription.Path); 

            Console.ReadLine(); 
        }
    }
}
