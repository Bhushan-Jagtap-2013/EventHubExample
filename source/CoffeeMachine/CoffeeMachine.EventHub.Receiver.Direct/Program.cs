using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Receiver.Direct
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ReadEventHubDataPerPartition().Wait();
            ReadEventHubData().Wait();
        }

        private static async Task ReadEventHubData()
        {
            string connectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            Console.WriteLine("Connecting to Event Hub..");
            EventHubConsumerClient eventHubClinet = new EventHubConsumerClient("$Default", connectionString);
            while (true)
            {
                Console.WriteLine("Waiting for data..");
                IAsyncEnumerable<PartitionEvent> partitionEvents = eventHubClinet.ReadEventsAsync();
                await foreach (PartitionEvent partitionEvent in partitionEvents)
                {
                    if (partitionEvent.Data != null)
                    {
                        Console.WriteLine("Event Data recieved {0} PartitionId: {1}", Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()), partitionEvent.Partition.PartitionId);
                        if (partitionEvent.Data.Properties != null)
                        {
                            foreach (var keyValue in partitionEvent.Data.Properties)
                            {
                                Console.WriteLine("Event data key = {0}, Event data value = {1}", keyValue.Key, keyValue.Value);
                            }
                        }
                    }
                }
            }
        }

        private static async Task ReadEventHubDataPerPartition()
        {
            string connectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            Console.WriteLine("Connecting to Event Hub..");
            EventHubConsumerClient eventHubClinet = new EventHubConsumerClient("$Default", connectionString);
            while (true)
            {
                Console.WriteLine("Waiting for data..");
                IAsyncEnumerable<PartitionEvent> partitionEvents = eventHubClinet.ReadEventsFromPartitionAsync("0", EventPosition.Latest);
                await foreach (PartitionEvent partitionEvent in partitionEvents)
                {
                    if (partitionEvent.Data != null)
                    {
                        Console.WriteLine("Event Data recieved {0} PartitionId: {1}", Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()), partitionEvent.Partition.PartitionId);
                        if (partitionEvent.Data.Properties != null)
                        {
                            foreach (var keyValue in partitionEvent.Data.Properties)
                            {
                                Console.WriteLine("Event data key = {0}, Event data value = {1}", keyValue.Key, keyValue.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}
