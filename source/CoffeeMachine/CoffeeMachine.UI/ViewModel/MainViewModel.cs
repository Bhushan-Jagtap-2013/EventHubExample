using CoffeeMachine.EventHub.Sender.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;

namespace CoffeeMachine.UI.ViewModel
{
    internal class MainViewModel : BindableBase
    {
        private int _counterMoca;
        private int _counterAmericano;
        private string _city;
        private string _serialNumber;

        public MainViewModel()
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 10);
            MakeMocaCommand = new DelegateCommand(MakeMoca);
            MakeAmericanoCommand = new DelegateCommand(MakeAmericano);
            City = GetCity();
        }

        private string GetCity()
        {
            Random random = new Random();
            string[] ListOfCity = new string[] { "Philadelphia", "Juneau", "Phoenix", "Little Rock" };
            return ListOfCity[(new Random()).Next(ListOfCity.Count())];
        }

        private void MakeAmericano()
        {
            CounterAmericano++;
            MachineData machineData = 
                InitializeMachineData(nameof(CounterAmericano), CounterAmericano);
        }

        private void MakeMoca()
        {
            CounterMoca++;
            MachineData machineData =
                InitializeMachineData(nameof(CounterMoca), CounterMoca);
        }

        public int CounterMoca {
            get => _counterMoca;
            set
            {
                _counterMoca = value;
                RaisePropertyChanged();
            }
        }

        public int CounterAmericano {
            get => _counterAmericano;
            set
            {
                _counterAmericano = value;
                RaisePropertyChanged();
            }
        }

        public string City { get => _city;
            set
            {
                _city = value;
                RaisePropertyChanged();
            }
        }
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand MakeMocaCommand { get; }
        public DelegateCommand MakeAmericanoCommand { get; }

        private MachineData InitializeMachineData(string sensorType, int sensorValue)
        {
            MachineData data = new MachineData();
            data.SerialNumber = SerialNumber;
            data.City = City;
            data.SensorType = sensorType;
            data.SensorValue = sensorValue;
            data.CreationTime = DateTime.Now;
            return data;
        }
    }
}
