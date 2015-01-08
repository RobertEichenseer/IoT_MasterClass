using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_EventConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            PartitionReader partitionReader = new PartitionReader();
            partitionReader.ReadFromEventHubPartition();
            //partitionReader.ReadUsingEventHubProcessor(); 
            
            Console.ReadLine(); 

        }
    }
}
