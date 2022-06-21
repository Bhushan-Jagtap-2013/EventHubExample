using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using System.Configuration;

namespace CoffeeMachine.EventHub.Receiver.Processor
{
    internal class Program
    {
        static BlobContainerClient storageClient;  
        static EventProcessorClient processor;

        static void Main(string[] args)
        {
            ProcessEventhubEvents().Wait();
        }

        static async Task ProcessEventhubEvents()
        {
            string eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            string blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            string blobContainerName = "eventhubcheckpoints";

            storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);
            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubConnectionString);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            Console.WriteLine("Start processing..");
            await processor.StartProcessingAsync();
            Console.WriteLine("Press any key..");
            Console.ReadLine();
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped processing..");
        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            try
            {
                Console.WriteLine("\tPartition Id {0}, Received event: {1}", eventArgs.Partition.PartitionId, Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Always catch exception or it will go unnoticed." + ex);
            }
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
