using CoffeeMachine.UI.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
