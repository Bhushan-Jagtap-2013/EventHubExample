using CoffeeMachine.EventHub.Sender;
using CoffeeMachine.EventHub.Sender.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CoffeeMachine.UI.ViewModel
{
    internal class MainViewModel : BindableBase
    {
        private int _counterMoca;
        private int _counterAmericano;
        private string _city;
        private string _serialNumber;
        private IMachineDataSender _machineDataSender;
        private int _boilerTemp;
        private int _beanLevel;
        private bool _isSending;
        private DispatcherTimer _timer;

        public MainViewModel(IMachineDataSender machineDataSender)
        {
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 10);
            MakeMocaCommand = new DelegateCommand(MakeMoca);
            MakeAmericanoCommand = new DelegateCommand(MakeAmericano);
            City = GetCity();
            this._machineDataSender = machineDataSender;
            Logs = new ObservableCollection<string>();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += DispatcherTimer_Tick;
        }

        private async void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            MachineData boilerTempData = InitializeMachineData(nameof(BoilerTemp), BoilerTemp);
            MachineData beanLevelData = InitializeMachineData(nameof(BeanLevel), BeanLevel);

            await SendDataAsync(new[] { boilerTempData, beanLevelData });
        }

        private string GetCity()
        {
            Random random = new Random();
            string[] ListOfCity = new string[] { "Philadelphia", "Juneau", "Phoenix", "Little Rock" };
            return ListOfCity[(new Random()).Next(ListOfCity.Count())];
        }

        private async void MakeAmericano()
        {
            CounterAmericano++;
            MachineData machineData = 
                InitializeMachineData(nameof(CounterAmericano), CounterAmericano);
            await SendDataAsync(machineData);
        }

        private async void MakeMoca()
        {
            CounterMoca++;
            MachineData machineData =
                InitializeMachineData(nameof(CounterMoca), CounterMoca);
            await SendDataAsync(machineData);
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
        public ObservableCollection<string> Logs { get; }
        public int BoilerTemp
        {
            get => _boilerTemp; set
            {
                _boilerTemp = value;
                RaisePropertyChanged();
            }
        }
        public int BeanLevel
        {
            get => _beanLevel; set
            {
                _beanLevel = value;
                RaisePropertyChanged();
            }
        }
        public bool IsSending
        {
            get => _isSending; set
            {
                if (_isSending != value)
                {
                    _isSending = value;
                    if(IsSending)
                    {
                        _timer.Start();
                    }
                    else
                    {
                        _timer.Stop();
                    }
                }
                RaisePropertyChanged();
            }
        }

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

        private async Task SendDataAsync(MachineData machineData)
        {
            try
            {
                await _machineDataSender.SendDataAsync(machineData);
                WriteLog("Sent to EventHub : " + machineData);
            }
            catch(Exception e)
            {
                WriteLog($"Exception: {e.Message}");
            }
        }

        private async Task SendDataAsync(IEnumerable<MachineData> machineDatas)
        {
            try
            {
                await _machineDataSender.SendDataAsync(machineDatas);
                foreach (MachineData data in machineDatas)
                {
                    WriteLog("Sent to EventHub : " + data);
                }
            }
            catch (Exception e)
            {
                WriteLog($"Exception: {e.Message}");
            }
        }

        private void WriteLog(string log)
        {
            Logs.Insert(0, log);
        }
    }
}
