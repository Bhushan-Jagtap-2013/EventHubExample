namespace CoffeeMachine.EventHub.Sender.Model
{
    public class MachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
