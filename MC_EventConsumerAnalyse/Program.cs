using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_EventConsumerAnalyse
{
    class Program
    {
        static void Main(string[] args)
        {

            AnalyticsEngine avgCalculation = new AnalyticsEngine();
            avgCalculation.StartAnalytics();
            PartitionReader partitionReader = new PartitionReader();
            partitionReader.ReadFromEventHubPartition();
            //partitionReader.ReadUsingEventHubProcessor(); 

            Console.ReadLine(); 
        }
    }
}
