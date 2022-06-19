using Newtonsoft.Json;

namespace CoffeeMachine.EventHub.Sender.Model
{
    public class MachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime CreationTime { get; set; }

        public override string ToString()
        {
            return String.Join(
                " | ", City, SerialNumber, SensorType, SensorValue, CreationTime);
        }
    }
}
