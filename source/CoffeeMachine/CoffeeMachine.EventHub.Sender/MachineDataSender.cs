using CoffeeMachine.EventHub.Sender.Model;
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
    }

    public class MachineDataSender : IMachineDataSender
    {
        public Task SendDataAsync(MachineData data)
        {
            throw new NotImplementedException();
        }
    }
}
