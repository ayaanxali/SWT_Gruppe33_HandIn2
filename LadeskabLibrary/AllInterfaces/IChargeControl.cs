using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabLibrary
{
    public interface IChargeControl
    {
        public bool IsConnected { get; set; }
        public void StartCharge();
        public void StopCharge();
    }

    public class ChargingEventArg : EventArgs
    {
        public bool Charging { get; set; }
    }
}
