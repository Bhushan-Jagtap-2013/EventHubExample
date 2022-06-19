using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using CoffeeMachine.EventHub.Sender.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Sender
{
    public interface IMachineDataSender
    {
        public Task SendDataAsync(MachineData data);
        public Task SendDataAsync(IEnumerable<MachineData> datas);
    }

    public class MachineDataSender : IMachineDataSender
    {
        EventHubProducerClient _eventHubProducerClient;
        public MachineDataSender(string connectionString)
        {
            _eventHubProducerClient = new EventHubProducerClient(connectionString);
        }

        public async Task SendDataAsync(MachineData data)
        {
            string dataAsJson = JsonConvert.SerializeObject(data);
            EventData eventData = new EventData(new BinaryData(dataAsJson));
            List<EventData> eventDataBatch = new List<EventData>();
            eventDataBatch.Add(eventData);
            await _eventHubProducerClient.SendAsync(eventDataBatch);
        }

        public async Task SendDataAsync(IEnumerable<MachineData> datas)
        {
            EventDataBatch eventBatch = await _eventHubProducerClient.CreateBatchAsync();

            for (int i = 1; i <= datas.Count(); i++)
            {
                string dataAsJson = JsonConvert.SerializeObject(datas);
                EventData eventData = new EventData(new BinaryData(dataAsJson));
                if (!eventBatch.TryAdd(eventData))
                {
                    await _eventHubProducerClient.SendAsync(eventBatch);
                    eventBatch = await _eventHubProducerClient.CreateBatchAsync();
                    eventBatch.TryAdd(eventData);
                }
            }

            if (eventBatch.Count > 0)
            {
                await _eventHubProducerClient.SendAsync(eventBatch);
            }
        }
    }
}
